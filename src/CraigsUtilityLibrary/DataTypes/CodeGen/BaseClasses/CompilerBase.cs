/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utilities.DataTypes.Patterns.BaseClasses;

namespace Utilities.DataTypes.CodeGen.BaseClasses
{
    /// <summary>
    /// Compiler base class
    /// </summary>
    public abstract class CompilerBase : SafeDisposableBaseClass
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblyName">Assembly name to save the generated types as</param>
        /// <param name="assemblyDirectory">Directory to save the generated types (optional)</param>
        /// <param name="optimize">Should this be optimized (defaults to true)</param>
        /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
        protected CompilerBase(string assemblyName, string assemblyDirectory = "", bool optimize = true, bool overwriteIfExists = false)
        {
            AssemblyDirectory = string.IsNullOrEmpty(assemblyDirectory) ?
                typeof(CompilerBase).Assembly.Location.Left(typeof(CompilerBase).Assembly.Location.LastIndexOf('\\')) :
                assemblyDirectory;
            AssemblyName = assemblyName;
            Optimize = optimize;
            Classes = new List<Type>();
            var CurrentFile = new FileInfo(AssemblyDirectory + "\\" + AssemblyName + ".dll");
            RegenerateAssembly = !CurrentFile.Exists || overwriteIfExists;
            if (string.IsNullOrEmpty(AssemblyDirectory)
                || !new FileInfo(AssemblyDirectory + "\\" + AssemblyName + ".dll").Exists
                || RegenerateAssembly)
            {
                AssemblyStream = new MemoryStream();
            }
            else
            {
                using (var TempFile = CurrentFile.OpenRead())
                {
                    var AssemblyBinary = TempFile.ReadAllBinary();
                    Assembly.Load(AssemblyBinary).GetTypes().ForEach(x => Classes.Add(x));
                }
            }
        }

        /// <summary>
        /// Assembly directory
        /// </summary>
        public string AssemblyDirectory { get; private set; }

        /// <summary>
        /// Assembly name
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Dictionary containing generated types and associates it with original type
        /// </summary>
        public ICollection<Type> Classes { get; private set; }

        /// <summary>
        /// Gets the assembly stream.
        /// </summary>
        /// <value>The assembly stream.</value>
        protected MemoryStream AssemblyStream { get; private set; }

        /// <summary>
        /// Should this be optimized?
        /// </summary>
        protected bool Optimize { get; private set; }

        /// <summary>
        /// Determines if the assembly needs to be regenerated
        /// </summary>
        protected bool RegenerateAssembly { get; private set; }

        /// <summary>
        /// Outputs basic information about the compiler as a string
        /// </summary>
        /// <returns>The string version of the compiler</returns>
        public override string ToString()
        {
            return "Compiler: " + AssemblyName + "\r\n";
        }

        /// <summary>
        /// Creates an object using the type specified
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="typeToCreate">Type to create</param>
        /// <param name="args">Args to pass to the constructor</param>
        /// <returns>The created object</returns>
        protected static T Create<T>(Type typeToCreate, params object[] args)
        {
            if (typeToCreate == null)
                throw new ArgumentNullException(nameof(typeToCreate));
            return (T)Activator.CreateInstance(typeToCreate, args);
        }

        /// <summary>
        /// Compiles and adds the item to the module
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="code">Code to compile</param>
        /// <param name="usings">Usings for the code</param>
        /// <param name="references">References to add for the compiler</param>
        /// <returns>This</returns>
        protected Type Add(string className, string code, IEnumerable<string> usings, params Assembly[] references)
        {
            if (AssemblyStream == null)
                return null;
            return Add(code, usings, references).FirstOrDefault(x => x.FullName == className);
        }

        /// <summary>
        /// Adds the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="usings">The usings.</param>
        /// <param name="references">The references.</param>
        /// <returns>The list of types that have been added</returns>
        /// <exception cref="System.Exception">Any errors that are sent back by Roslyn</exception>
        protected IEnumerable<Type> Add(string code, IEnumerable<string> usings, params Assembly[] references)
        {
            if (AssemblyStream == null)
                return null;
            CSharpCompilation CSharpCompiler = CSharpCompilation.Create(AssemblyName + ".dll",
                                                    new SyntaxTree[] { CSharpSyntaxTree.ParseText(code) },
                                                    references.ForEach(x => MetadataReference.CreateFromFile(x.Location)),
                                                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, usings: usings, optimizationLevel: Optimize ? OptimizationLevel.Release : OptimizationLevel.Debug));
            using (MemoryStream TempStream = new MemoryStream())
            {
                EmitResult Result = CSharpCompiler.Emit(TempStream);
                if (!Result.Success)
                    throw new Exception(code + Environment.NewLine + Environment.NewLine + Result.Diagnostics.ToString(x => x.GetMessage() + " : " + x.Location.GetLineSpan().StartLinePosition.Line, Environment.NewLine));
                byte[] MiniAssembly = TempStream.ToArray();
                Classes.AddIfUnique((x, y) => x.FullName == y.FullName, AppDomain.CurrentDomain.Load(MiniAssembly).GetTypes());
                AssemblyStream.Write(MiniAssembly, 0, MiniAssembly.Length);
            }
            return Classes;
        }

        /// <summary>
        /// Compiles and adds the item to the module
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="code">Code to compile</param>
        /// <param name="references">References to add for the compiler</param>
        /// <returns>This</returns>
        protected Type Add(string className, string code, params Assembly[] references)
        {
            return Add(className, code, null, references);
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">Destroy managed</param>
        protected override void Dispose(bool Managed)
        {
            Save();
            if (AssemblyStream != null)
            {
                AssemblyStream.Dispose();
                AssemblyStream = null;
            }
            Classes = new List<Type>();
        }

        /// <summary>
        /// Saves the assembly
        /// </summary>
        protected void Save()
        {
            if ((AssemblyStream != null
                && !string.IsNullOrEmpty(AssemblyDirectory)
                && (!new FileInfo(AssemblyDirectory + "\\" + AssemblyName + ".dll").Exists
                || RegenerateAssembly))
                && AssemblyStream.Length > 0)
            {
                using (FileStream TempStream = new FileInfo(AssemblyDirectory + "\\" + AssemblyName + ".dll").OpenWrite())
                {
                    byte[] TempArray = AssemblyStream.ToArray();
                    TempStream.Write(TempArray, 0, TempArray.Length);
                }
            }
        }
    }
}