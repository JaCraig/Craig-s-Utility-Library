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

using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;

namespace Utilities.Random.NameGenerators
{
    /// <summary>
    /// Male first name generator
    /// </summary>
    public class MaleFirstNameGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        private string[] MaleFirstNames = { "Jacob", "Mason", "William", "Jayden", "Noah", "Michael", "Ethan",
                                              "Alexander", "Aiden", "Daniel", "Anthony", "Matthew", "Elijah", "Joshua",
                                              "Liam", "Andrew", "James", "David", "Benjamin", "Logan", "Christopher", "Joseph",
                                              "Jackson", "Gabriel", "Ryan", "Samuel", "John", "Nathan", "Lucas", "Christian",
                                              "Jonathan", "Caleb", "Dylan", "Landon", "Isaac", "Gavin", "Brayden", "Tyler",
                                              "Luke", "Evan", "Carter", "Nicholas", "Isaiah", "Owen", "Jack", "Jordan",
                                              "Brandon", "Wyatt", "Julian", "Aaron", "Jeremiah", "Angel", "Cameron", "Connor",
                                              "Hunter", "Adrian", "Henry", "Eli", "Justin", "Austin", "Robert", "Charles",
                                              "Thomas", "Zachary", "Jose", "Levi", "Kevin", "Sebastian", "Chase", "Ayden",
                                              "Jason", "Ian", "Blake", "Colton", "Bentley", "Dominic", "Xavier", "Oliver",
                                              "Parker", "Josiah", "Adam", "Cooper", "Brody", "Nathaniel", "Carson", "Jaxon",
                                              "Tristan", "Luis", "Juan", "Hayden", "Carlos", "Jesus", "Nolan", "Cole", "Alex",
                                              "Max", "Grayson", "Bryson", "Diego", "Jaden", "Vincent", "Easton", "Eric", "Micah",
                                              "Kayden", "Jace", "Aidan", "Ryder", "Ashton", "Bryan", "Riley", "Hudson", "Asher",
                                              "Bryce", "Miles", "Kaleb", "Giovanni", "Antonio", "Kaden", "Colin", "Kyle",
                                              "Brian", "Timothy", "Steven", "Sean", "Miguel", "Richard", "Ivan", "Jake",
                                              "Alejandro", "Santiago", "Axel", "Joel", "Maxwell", "Brady", "Caden", "Preston",
                                              "Damian", "Elias", "Jaxson", "Jesse", "Victor", "Patrick", "Jonah", "Marcus",
                                              "Rylan", "Emmanuel", "Edward", "Leonardo", "Cayden", "Grant", "Jeremy", "Braxton",
                                              "Gage", "Jude", "Wesley", "Devin", "Roman", "Mark", "Camden", "Kaiden", "Oscar",
                                              "Alan", "Malachi", "George", "Peyton", "Leo", "Nicolas", "Maddox", "Kenneth",
                                              "Mateo", "Sawyer", "Collin", "Conner", "Cody", "Andres", "Declan", "Lincoln",
                                              "Bradley", "Trevor", "Derek", "Tanner", "Silas", "Eduardo", "Seth", "Jaiden",
                                              "Paul", "Jorge", "Cristian", "Garrett", "Travis", "Abraham", "Omar", "Javier",
                                              "Ezekiel", "Tucker", "Harrison", "Peter", "Damien", "Greyson", "Avery", "Kai",
                                              "Weston", "Ezra", "Xander", "Jaylen", "Corbin", "Fernando", "Calvin", "Jameson",
                                              "Francisco", "Maximus", "Josue", "Ricardo", "Shane", "Trenton", "Cesar", "Chance",
                                              "Drake", "Zane", "Israel", "Emmett", "Jayce", "Mario", "Landen", "Kingston",
                                              "Spencer", "Griffin", "Stephen", "Manuel", "Theodore", "Erick", "Braylon",
                                              "Raymond", "Edwin", "Charlie", "Abel", "Myles", "Bennett", "Johnathan", "Andre",
                                              "Alexis", "Edgar", "Troy", "Zion", "Jeffrey", "Hector", "Shawn", "Lukas", "Amir" };

        /// <summary>
        /// Constructor
        /// </summary>
        public MaleFirstNameGenerator()
            : base("", "")
        {
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public virtual string Next(System.Random Rand)
        {
            return Rand.Next(MaleFirstNames);
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public virtual string Next(System.Random Rand, string Min, string Max)
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
    }
}