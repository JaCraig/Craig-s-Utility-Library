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
    /// Female first name generator
    /// </summary>
    public class FemaleFirstNameGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        private string[] FemaleFirstNames = { "Sophia", "Isabella", "Emma", "Olivia", "Ava", "Emily",
                                                "Abigail", "Madison", "Mia", "Chloe", "Elizabeth",
                                                "Ella", "Addison", "Natalie", "Lily", "Grace", "Samantha"
                                                , "Avery", "Sofia", "Aubrey", "Brooklyn", "Lillian",
                                                "Victoria", "Evelyn", "Hannah", "Alexis", "Charlotte",
                                                "Zoey", "Leah", "Amelia", "Zoe", "Hailey", "Layla",
                                                "Gabriella", "Nevaeh", "Kaylee", "Alyssa", "Anna", "Sarah",
                                                "Allison", "Savannah", "Ashley", "Audrey", "Taylor",
                                                "Brianna", "Aaliyah", "Riley", "Camila", "Khloe", "Claire",
                                                "Sophie", "Arianna", "Peyton", "Harper", "Alexa", "Makayla",
                                                "Julia", "Kylie", "Kayla", "Bella", "Katherine", "Lauren",
                                                "Gianna", "Maya", "Sydney", "Serenity", "Kimberly", "Mackenzie",
                                                "Autumn", "Jocelyn", "Faith", "Lucy", "Stella", "Jasmine",
                                                "Morgan", "Alexandra", "Trinity", "Molly", "Madelyn",
                                                "Scarlett", "Andrea", "Genesis", "Eva", "Ariana", "Madeline",
                                                "Brooke", "Caroline", "Bailey", "Melanie", "Kennedy",
                                                "Destiny", "Maria", "Naomi", "London", "Payton", "Lydia",
                                                "Ellie", "Mariah", "Aubree", "Kaitlyn", "Violet", "Rylee",
                                                "Lilly", "Angelina", "Katelyn", "Mya", "Paige", "Natalia",
                                                "Ruby", "Piper", "Annabelle", "Mary", "Jade", "Isabelle",
                                                "Liliana", "Nicole", "Rachel", "Vanessa", "Gabrielle", "Jessica",
                                                "Jordyn", "Reagan", "Kendall", "Sadie", "Valeria", "Brielle",
                                                "Lyla", "Isabel", "Brooklynn", "Reese", "Sara", "Adriana",
                                                "Aliyah", "Jennifer", "Mckenzie", "Gracie", "Nora", "Kylee",
                                                "Makenzie", "Izabella", "Laila", "Alice", "Amy", "Michelle",
                                                "Skylar", "Stephanie", "Juliana", "Rebecca", "Jayla", "Eleanor",
                                                "Clara", "Giselle", "Valentina", "Vivian", "Alaina", "Eliana",
                                                "Aria", "Valerie", "Haley", "Elena", "Catherine", "Elise", "Lila",
                                                "Megan", "Gabriela", "Daisy", "Jada", "Daniela", "Penelope",
                                                "Jenna", "Ashlyn", "Delilah", "Summer", "Mila", "Kate", "Keira",
                                                "Adrianna", "Hadley", "Julianna", "Maci", "Eden", "Josephine",
                                                "Aurora", "Melissa", "Hayden", "Alana", "Margaret", "Quinn",
                                                "Angela", "Brynn", "Alivia", "Katie", "Ryleigh", "Kinley",
                                                "Paisley", "Jordan", "Aniyah", "Allie", "Miranda", "Jacquelin",
                                                "Melody", "Willow", "Diana", "Cora", "Alexandria", "Mikayla",
                                                "Danielle", "Londyn", "Addyson", "Amaya", "Hazel", "Callie",
                                                "Teagan", "Adalyn", "Ximena", "Angel", "Kinsley", "Shelby",
                                                "Makenna", "Ariel", "Jillian", "Chelsea", "Alayna", "Harmony",
                                                "Sienna", "Amanda", "Presley", "Maggie", "Tessa", "Leila", "Hope",
                                                "Genevieve", "Erin", "Briana", "Delaney", "Esther", "Kathryn",
                                                "Ana", "Mckenna", "Camille", "Cecilia", "Lucia", "Lola", "Leilani",
                                                "Leslie", "Ashlynn", "Kayleigh", "Alondra", "Alison", "Haylee" };

        /// <summary>
        /// Constructor
        /// </summary>
        public FemaleFirstNameGenerator()
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
            return Rand.Next(FemaleFirstNames);
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