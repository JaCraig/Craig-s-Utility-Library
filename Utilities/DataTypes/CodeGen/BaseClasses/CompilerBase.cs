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

#region Usings
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Utilities.DataTypes.Patterns.BaseClasses;
#endregion

namespace Utilities.DataTypes.CodeGen.BaseClasses
{
    /// <summary>
    /// Compiler base class
    /// </summary>
    public abstract class CompilerBase : SafeDisposableBaseClass
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AssemblyDirectory">Directory to save the generated types (optional)</param>
        /// <param name="AssemblyName">Assembly name to save the generated types as</param>
        /// <param name="Optimize">Should this be optimized (defaults to true)</param>
        protected CompilerBase(string AssemblyName, string AssemblyDirectory = "", bool Optimize = true)
        {
            this.AssemblyDirectory = string.IsNullOrEmpty(AssemblyDirectory) ?
                typeof(CompilerBase).Assembly.Location.Left(typeof(CompilerBase).Assembly.Location.LastIndexOf('\\')) :
                AssemblyDirectory;
            this.AssemblyName = AssemblyName;
            this.Optimize = Optimize;
            this.Classes = new List<Type>();
            System.IO.FileInfo CurrentFile = new System.IO.FileInfo(this.AssemblyDirectory + "\\" + this.AssemblyName + ".dll");
            this.RegenerateAssembly = (!CurrentFile.Exists
                                      || AppDomain.CurrentDomain.GetAssemblies()
                                                                .Where(x => !x.FullName.Contains("vshost32"))
                                                                .Any(x => new System.IO.FileInfo(x.Location).LastWriteTime > CurrentFile.LastWriteTime));
            if (string.IsNullOrEmpty(this.AssemblyDirectory)
                || !new System.IO.FileInfo(this.AssemblyDirectory + "\\" + this.AssemblyName + ".dll").Exists
                || this.RegenerateAssembly)
            {
                this.Assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(this.AssemblyName), AssemblyBuilderAccess.RunAndSave, this.AssemblyDirectory);
                this.Module = Assembly.DefineDynamicModule(this.AssemblyName, this.AssemblyName + ".dll", true);
            }
            else
            {
                System.Reflection.Assembly.Load(new AssemblyName(new FileInfo(this.AssemblyDirectory + "\\" + this.AssemblyName + ".dll").FullName)).GetTypes().ForEach(x => this.Classes.Add(x));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Assembly builder
        /// </summary>
        protected AssemblyBuilder Assembly { get; private set; }

        /// <summary>
        /// Module builder
        /// </summary>
        protected ModuleBuilder Module { get; private set; }

        /// <summary>
        /// Dictionary containing generated types and associates it with original type
        /// </summary>
        public ICollection<Type> Classes { get; private set; }

        /// <summary>
        /// Assembly directory
        /// </summary>
        public string AssemblyDirectory { get; private set; }

        /// <summary>
        /// Assembly name
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Determines if the assembly needs to be regenerated
        /// </summary>
        protected bool RegenerateAssembly { get; private set; }

        /// <summary>
        /// Should this be optimized?
        /// </summary>
        protected bool Optimize { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Creates an object using the type specified
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="TypeToCreate">Type to create</param>
        /// <param name="Args">Args to pass to the constructor</param>
        /// <returns>The created object</returns>
        protected static T Create<T>(Type TypeToCreate, params object[] Args)
        {
            Contract.Requires<ArgumentNullException>(TypeToCreate != null, "TypeToCreate");
            return (T)Activator.CreateInstance(TypeToCreate, Args);
        }

        /// <summary>
        /// Compiles and adds the item to the module
        /// </summary>
        /// <param name="ClassName">Class name</param>
        /// <param name="Code">Code to compile</param>
        /// <param name="Usings">Usings for the code</param>
        /// <param name="References">References to add for the compiler</param>
        /// <returns>This</returns>
        protected Type Add(string ClassName, string Code, IEnumerable<string> Usings, params Assembly[] References)
        {
            if (Module == null)
                return null;
            Compilation CSharpCompiler = Compilation.Create(AssemblyName + ".dll",
                new CompilationOptions(OutputKind.DynamicallyLinkedLibrary, usings: Usings, optimize: Optimize),
                                                    new SyntaxTree[] { SyntaxTree.ParseText(Code) },
                                                    References.ForEach(x => new MetadataFileReference(x.Location)).ToArray());
            ReflectionEmitResult Result = CSharpCompiler.Emit(Module);
            if (!Result.Success)
                throw new Exception(Result.Diagnostics.ToString(x => x.Info.GetMessage(), System.Environment.NewLine));
            Type ReturnType = Module.GetTypes().FirstOrDefault(x => x.FullName == ClassName);
            Classes.Add(ReturnType);
            return ReturnType;
        }

        /// <summary>
        /// Compiles and adds the item to the module
        /// </summary>
        /// <param name="ClassName">Class name</param>
        /// <param name="Code">Code to compile</param>
        /// <param name="References">References to add for the compiler</param>
        /// <returns>This</returns>
        protected Type Add(string ClassName, string Code, params Assembly[] References)
        {
            return Add(ClassName, Code, null, References);
        }

        /// <summary>
        /// Saves the assembly
        /// </summary>
        protected void Save()
        {
            if (Assembly != null
                && !string.IsNullOrEmpty(AssemblyDirectory)
                && (!new FileInfo(AssemblyDirectory + "\\" + AssemblyName + ".dll").Exists
                || RegenerateAssembly))
            {
                Assembly.Save(AssemblyName + ".dll");
            }
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">Destroy managed</param>
        protected override void Dispose(bool Managed)
        {
            Save();
            Assembly = null;
            Module = null;
            Classes = new List<Type>();
        }

        #endregion
    }
}