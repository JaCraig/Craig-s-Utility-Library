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
using Utilities.DataTypes;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;

namespace Utilities.Random.NameGenerators
{
    /// <summary>
    /// Last name generator
    /// </summary>
    public class LastNameGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        private string[] LastNames = { "SMITH", "JOHNSON", "WILLIAMS", "JONES", "BROWN", "DAVIS", "MILLER", "WILSON", "MOORE", "TAYLOR", "ANDERSON", "THOMAS", "JACKSON", "WHITE", "HARRIS", "MARTIN", "THOMPSON", "GARCIA", "MARTINEZ", "ROBINSON", "CLARK", "RODRIGUEZ", "LEWIS", "LEE", "WALKER", "HALL", "ALLEN", "YOUNG", "HERNANDEZ", "KING", "WRIGHT", "LOPEZ", "HILL", "SCOTT", "GREEN", "ADAMS", "BAKER", "GONZALEZ", "NELSON", "CARTER", "MITCHELL", "PEREZ", "ROBERTS", "TURNER", "PHILLIPS", "CAMPBELL", "PARKER", "EVANS", "EDWARDS", "COLLINS", "STEWART", "SANCHEZ", "MORRIS", "ROGERS", "REED", "COOK", "MORGAN", "BELL", "MURPHY", "BAILEY", "RIVERA", "COOPER", "RICHARDSON", "COX", "HOWARD", "WARD", "TORRES", "PETERSON", "GRAY", "RAMIREZ", "JAMES", "WATSON", "BROOKS", "KELLY", "SANDERS", "PRICE", "BENNETT", "WOOD", "BARNES", "ROSS", "HENDERSON", "COLEMAN", "JENKINS", "PERRY", "POWELL", "LONG", "PATTERSON", "HUGHES", "FLORES", "WASHINGTON", "BUTLER", "SIMMONS", "FOSTER", "GONZALES", "BRYANT", "ALEXANDER", "RUSSELL", "GRIFFIN", "DIAZ", "HAYES", "MYERS", "FORD", "HAMILTON", "GRAHAM", "SULLIVAN", "WALLACE", "WOODS", "COLE", "WEST", "JORDAN", "OWENS", "REYNOLDS", "FISHER", "ELLIS", "HARRISON", "GIBSON", "MCDONALD", "CRUZ", "MARSHALL", "ORTIZ", "GOMEZ", "MURRAY", "FREEMAN", "WELLS", "WEBB", "SIMPSON", "STEVENS", "TUCKER", "PORTER", "HUNTER", "HICKS", "CRAWFORD", "HENRY", "BOYD", "MASON", "MORALES", "KENNEDY", "WARREN", "DIXON", "RAMOS", "REYES", "BURNS", "GORDON", "SHAW", "HOLMES", "RICE", "ROBERTSON", "HUNT", "BLACK", "DANIELS", "PALMER", "MILLS", "NICHOLS", "GRANT", "KNIGHT", "FERGUSON", "ROSE", "STONE", "HAWKINS", "DUNN", "PERKINS", "HUDSON", "SPENCER", "GARDNER", "STEPHENS", "PAYNE", "PIERCE", "BERRY", "MATTHEWS", "ARNOLD", "WAGNER", "WILLIS", "RAY", "WATKINS", "OLSON", "CARROLL", "DUNCAN", "SNYDER", "HART", "CUNNINGHAM", "BRADLEY", "LANE", "ANDREWS", "RUIZ", "HARPER", "FOX", "RILEY", "ARMSTRONG", "CARPENTER", "WEAVER", "GREENE", "LAWRENCE", "ELLIOTT", "CHAVEZ", "SIMS", "AUSTIN", "PETERS", "KELLEY", "FRANKLIN", "LAWSON", "FIELDS", "GUTIERREZ", "RYAN", "SCHMIDT", "CARR", "VASQUEZ", "CASTILLO", "WHEELER", "CHAPMAN", "OLIVER", "MONTGOMERY", "RICHARDS", "WILLIAMSON", "JOHNSTON", "BANKS", "MEYER", "BISHOP", "MCCOY", "HOWELL", "ALVAREZ", "MORRISON", "HANSEN", "FERNANDEZ", "GARZA", "HARVEY", "LITTLE", "BURTON", "STANLEY", "NGUYEN", "GEORGE", "JACOBS", "REID", "KIM", "FULLER", "LYNCH", "DEAN", "GILBERT", "GARRETT", "ROMERO", "WELCH", "LARSON", "FRAZIER", "BURKE", "HANSON", "DAY", "MENDOZA", "MORENO", "BOWMAN", "MEDINA", "FOWLER", "BREWER", "HOFFMAN", "CARLSON", "SILVA", "PEARSON", "HOLLAND", "DOUGLAS", "FLEMING", "JENSEN", "VARGAS", "BYRD", "DAVIDSON", "HOPKINS", "MAY", "TERRY", "HERRERA", "WADE", "SOTO", "WALTERS", "CURTIS", "NEAL", "CALDWELL", "LOWE", "JENNINGS", "BARNETT", "GRAVES", "JIMENEZ", "HORTON", "SHELTON", "BARRETT", "OBRIEN", "CASTRO", "SUTTON", "GREGORY", "MCKINNEY", "LUCAS", "MILES", "CRAIG", "RODRIQUEZ", "CHAMBERS", "HOLT", "LAMBERT", "FLETCHER", "WATTS", "BATES", "HALE", "RHODES", "PENA", "BECK", "NEWMAN", "HAYNES", "MCDANIEL", "MENDEZ", "BUSH", "VAUGHN", "PARKS", "DAWSON", "SANTIAGO", "NORRIS", "HARDY", "LOVE", "STEELE", "CURRY", "POWERS", "SCHULTZ", "BARKER", "GUZMAN", "PAGE", "MUNOZ", "BALL", "KELLER", "CHANDLER", "WEBER", "LEONARD", "WALSH", "LYONS", "RAMSEY", "WOLFE", "SCHNEIDER", "MULLINS", "BENSON", "SHARP", "BOWEN", "DANIEL", "BARBER", "CUMMINGS", "HINES", "BALDWIN", "GRIFFITH", "VALDEZ", "HUBBARD", "SALAZAR", "REEVES", "WARNER", "STEVENSON", "BURGESS", "SANTOS", "TATE", "CROSS", "GARNER", "MANN", "MACK", "MOSS", "THORNTON", "DENNIS", "MCGEE", "FARMER", "DELGADO", "AGUILAR", "VEGA", "GLOVER", "MANNING", "COHEN", "HARMON", "RODGERS", "ROBBINS", "NEWTON", "TODD", "BLAIR", "HIGGINS", "INGRAM", "REESE", "CANNON", "STRICKLAND", "TOWNSEND", "POTTER", "GOODWIN", "WALTON", "ROWE", "HAMPTON", "ORTEGA", "PATTON", "SWANSON", "JOSEPH", "FRANCIS", "GOODMAN", "MALDONADO", "YATES", "BECKER", "ERICKSON", "HODGES", "RIOS", "CONNER", "ADKINS", "WEBSTER", "NORMAN", "MALONE", "HAMMOND", "FLOWERS", "COBB", "MOODY", "QUINN", "BLAKE", "MAXWELL", "POPE", "FLOYD", "OSBORNE", "PAUL", "MCCARTHY", "GUERRERO", "LINDSEY", "ESTRADA", "SANDOVAL", "GIBBS", "TYLER", "GROSS", "FITZGERALD", "STOKES", "DOYLE", "SHERMAN", "SAUNDERS", "WISE", "COLON", "GILL", "ALVARADO", "GREER", "PADILLA", "SIMON", "WATERS", "NUNEZ", "BALLARD", "SCHWARTZ", "MCBRIDE", "HOUSTON", "CHRISTENSEN", "KLEIN", "PRATT", "BRIGGS", "PARSONS", "MCLAUGHLIN", "ZIMMERMAN", "FRENCH", "BUCHANAN", "MORAN", "COPELAND", "ROY", "PITTMAN", "BRADY", "MCCORMICK", "HOLLOWAY", "BROCK", "POOLE", "FRANK", "LOGAN", "OWEN", "BASS", "MARSH", "DRAKE", "WONG", "JEFFERSON", "PARK", "MORTON", "ABBOTT", "SPARKS", "PATRICK", "NORTON", "HUFF", "CLAYTON", "MASSEY", "LLOYD", "FIGUEROA", "CARSON", "BOWERS", "ROBERSON", "BARTON", "TRAN", "LAMB", "HARRINGTON", "CASEY", "BOONE", "CORTEZ", "CLARKE", "MATHIS", "SINGLETON", "WILKINS", "CAIN", "BRYAN", "UNDERWOOD", "HOGAN", "MCKENZIE", "COLLIER", "LUNA", "PHELPS", "MCGUIRE", "ALLISON", "BRIDGES", "WILKERSON", "NASH", "SUMMERS" };

        /// <summary>
        /// Constructor
        /// </summary>
        public LastNameGenerator()
            : base("", "")
        {
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public virtual string Next(System.Random Rand)
        {
            return Rand.Next(LastNames).ToLower(CultureInfo.InvariantCulture).ToString(StringCase.FirstCharacterUpperCase);
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