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
using System;
using System.Speech.Synthesis;
using System.Threading;

#endregion

namespace Utilities.Media.Sound
{
    /// <summary>
    /// Speech to text helper
    /// </summary>
    public class SpeechToText:IDisposable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SpeechToText()
        {
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Converts text to speech
        /// </summary>
        /// <param name="Text">Text to convert</param>
        public void Speak(string Text)
        {
            this.Text=Text;
            Thread TempThread = new Thread(new ThreadStart(SpeakAsync));
            TempThread.SetApartmentState(ApartmentState.STA);
            TempThread.Start();
            TempThread.Join();
        }

        /// <summary>
        /// Converts text to speech
        /// </summary>
        /// <param name="Text">Text to convert</param>
        /// <param name="OutputFile">Output file</param>
        public void Speak(string Text, string OutputFile)
        {
            this.Text = Text;
            this.OutputFile = OutputFile;
            Thread TempThread = new Thread(new ThreadStart(SpeakAsync));
            TempThread.SetApartmentState(ApartmentState.STA);
            TempThread.Start();
            TempThread.Join();
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Converts text to speech
        /// </summary>
        void SpeakAsync()
        {
            if (!string.IsNullOrEmpty(OutputFile))
            {
                this.Synthesizer.SetOutputToWaveFile(OutputFile);
            }
            this.Synthesizer.SpeakAsync(Text);
            if (!string.IsNullOrEmpty(OutputFile))
            {
                this.Synthesizer.SetOutputToDefaultAudioDevice();
            }
            this.Text = "";
            this.OutputFile = "";
        }

        #endregion

        #region Private Variables

        private SpeechSynthesizer Synthesizer=new SpeechSynthesizer();
        private string Text="";
        private string OutputFile = "";

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (Synthesizer != null)
            {
                Synthesizer.Dispose();
                Synthesizer = null;
            }
        }

        #endregion
    }
}
