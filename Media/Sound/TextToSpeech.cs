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
using System.Speech.Recognition;
using System.Text;
using System.Threading;
#endregion

namespace Utilities.Media.Sound
{
    /// <summary>
    /// Text to speech helper
    /// </summary>
    public class TextToSpeech
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TextToSpeech()
        {
        }

        #endregion

        #region Public Functions


        /// <summary>
        /// Recognizes text from speech
        /// </summary>
        /// <param name="InputFile">Wave file to use</param>
        /// <returns>The text found in the wave file</returns>
        public string Recognize(string InputFile)
        {
            this.InputFile = InputFile;
            Thread TempThread = new Thread(new ThreadStart(RecognizeAsync));
            TempThread.SetApartmentState(ApartmentState.STA);
            TempThread.Start();
            TempThread.Join();
            return Text;
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Recognizes speech
        /// </summary>
        void RecognizeAsync()
        {
            try
            {
                RecognitionEngine.LoadGrammar(new DictationGrammar());
                RecognitionResult Result = RecognitionEngine.Recognize();
                StringBuilder Output = new StringBuilder();
                foreach (RecognizedWordUnit Word in Result.Words)
                {
                    Output.Append(Word.Text);
                }
                Text = Output.ToString();
            }
            catch { }
        }

        #endregion

        #region Private Variables

        private SpeechRecognitionEngine RecognitionEngine = new SpeechRecognitionEngine();
        private string InputFile = "";
        private string Text="";

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (RecognitionEngine != null)
            {
                RecognitionEngine.Dispose();
                RecognitionEngine = null;
            }
        }

        #endregion
    }
}
