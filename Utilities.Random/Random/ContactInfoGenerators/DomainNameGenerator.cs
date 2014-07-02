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

using System.Globalization;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;
using Utilities.Random.NameGenerators;

namespace Utilities.Random.ContactInfoGenerators
{
    /// <summary>
    /// Generates a random domain name
    /// </summary>
    public class DomainNameGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        private string[] Endings = { ".ag", ".am", ".as", ".at", ".az", ".be", ".bi", ".bs", ".cc", ".cf", ".cg", ".ch", ".co.at", ".co.ck", ".co.gg", ".co.il", ".co.je", ".co.ma", ".co.mu", ".co.mz", ".co.nz", ".co.pn", ".co.ro", ".co.tt", ".co.uk", ".co.vi", ".co.za", ".com", ".com.ag", ".com.ar", ".com.az", ".com.bs", ".com.dm", ".com.do", ".com.ec", ".com.fj", ".com.gd", ".com.gi", ".com.gt", ".com.gy", ".com.jm", ".com.kh", ".com.kn", ".com.lc", ".com.lk", ".com.lv", ".com.ly", ".com.mx", ".com.nf", ".com.ni", ".com.pa", ".com.pe", ".com.ph", ".com.pl", ".com.pr", ".com.pt", ".com.ro", ".com.ru", ".com.sb", ".com.sc", ".com.tj", ".com.tp", ".com.ua", ".com.ve", ".cx", ".cz", ".dk", ".fm", ".gd", ".gen.tr", ".gg", ".gl", ".gs", ".gy", ".hm", ".io", ".je", ".jp", ".kg", ".kn", ".kz", ".li", ".lk", ".lt", ".lv", ".ly", ".ma", ".md", ".ms", ".mu", ".mw", ".net", ".net.tp", ".nu", ".off.ai", ".org", ".org.tp", ".org.uk", ".ph", ".pl", ".ro", ".ru", ".rw", ".sc", ".sh", ".sn", ".st", ".tc", ".tf", ".tj", ".to", ".tp", ".tt", ".uz", ".vg", ".vu", ".ws" };

        private string[] MostCommonEndings = { ".com", ".net", ".org" };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CommonEndings">
        /// Common endings to domain names should be used (.com,.org,.net,etc.)
        /// </param>
        public DomainNameGenerator(bool CommonEndings = true)
            : base("", "")
        {
            this.CommonEndings = CommonEndings;
        }

        /// <summary>
        /// Should common domain name endings be used
        /// </summary>
        public virtual bool CommonEndings { get; private set; }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public string Next(System.Random Rand)
        {
            string CompanyName = new CompanyGenerator().Next(Rand);
            return ((CompanyName.Length > 10) ? CleanName(CompanyName.Split(' ')[0]) : CleanName(CompanyName))
                + (CommonEndings ? Rand.Next(MostCommonEndings) : Rand.Next(Endings));
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public string Next(System.Random Rand, string Min, string Max)
        {
            return Next(Rand);
        }

        /// <summary>
        /// Generates next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return Next(Rand);
        }

        private static string CleanName(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return Name;
            return Name.ToLower(CultureInfo.InvariantCulture).Replace(" ", "").Replace(",", "").Replace("'", "").Replace("&", "").Replace(".", "");
        }
    }
}