/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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

#endregion

namespace Utilities.Math
{
    /// <summary>
    /// Conversion helper
    /// </summary>
    public class Conversion
    {
        #region Public Static Function

        /// <summary>
        /// Celsius to Fahrenheit
        /// </summary>
        /// <param name="Value">Celsius value</param>
        /// <returns>Equivalent Fahrenheit temp</returns>
        public static double CelsiusToFahrenheit(double Value)
        {
            return ((Value * 9) / 5) + 32;
        }

        /// <summary>
        /// Celsius to Kelvin
        /// </summary>
        /// <param name="Value">Celsius value</param>
        /// <returns>Equivalent Kelvin temp</returns>
        public static double CelsiusToKelvin(double Value)
        {
            return Value + 273.15;
        }

        /// <summary>
        /// Fahrenheit to Celsius
        /// </summary>
        /// <param name="Value">Fahrenheit value</param>
        /// <returns>Equivalent Celsius value</returns>
        public static double FahrenheitToCelsius(double Value)
        {
            return ((Value - 32) * 5) / 9;
        }

        /// <summary>
        /// Fahrenheit to Kelvin
        /// </summary>
        /// <param name="Value">Fahrenheit value</param>
        /// <returns>Equivalent Kelvin value</returns>
        public static double FahrenheitToKelvin(double Value)
        {
            return ((Value + 459.67) * 5) / 9;
        }

        /// <summary>
        /// Kelvin to Celsius
        /// </summary>
        /// <param name="Value">Kelvin value</param>
        /// <returns>Equivalent Celsius value</returns>
        public static double KelvinToCelsius(double Value)
        {
            return Value - 273.15;
        }

        /// <summary>
        /// Kelvin to Fahrenheit
        /// </summary>
        /// <param name="Value">Kelvin value</param>
        /// <returns>Equivalent Fahrenheit value</returns>
        public static double KelvinToFahrenheit(double Value)
        {
            return ((Value * 9) / 5) - 459.67;
        }

        #endregion
    }
}
