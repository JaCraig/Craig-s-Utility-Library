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
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace Utilities.Web.Email.Pop3
{
    /// <summary>
    /// Class for implemented basic Pop3 client
    /// functionality.
    /// </summary>
    public class Pop3Client : TcpClient
    {
        #region Public Functions

        /// <summary>
        /// Connects to a server
        /// </summary>
        /// <param name="UserName">Username used to log into the server</param>
        /// <param name="Password">Password used to log into the server</param>
        /// <param name="Server">Server location</param>
        /// <param name="Port">Port on the server to use</param>
        public void Connect(string UserName, string Password, string Server, int Port)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.Server = Server;
            this.Port = Port;
            string ResponseString;

            Connect(Server, Port);
            ResponseString = GetResponse();
            if (!ResponseString.StartsWith("+OK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Pop3Exception(ResponseString);
            }

            WriteMessage("USER " + UserName + "\r\n");
            ResponseString = GetResponse();
            if (!ResponseString.StartsWith("+OK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Pop3Exception(ResponseString);
            }

            WriteMessage("PASS " + Password + "\r\n");
            ResponseString = GetResponse();
            if (!ResponseString.StartsWith("+OK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Pop3Exception(ResponseString);
            }
        }

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        public void Disconnect()
        {
            string ResponseString;
            WriteMessage("QUIT\r\n");
            ResponseString = GetResponse();
            if (!ResponseString.StartsWith("+OK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Pop3Exception(ResponseString);
            }
        }

        /// <summary>
        /// Gets a list of messages from the server
        /// </summary>
        /// <returns>A list of messages (only contains message number and size)</returns>
        public List<Message> GetMessageList()
        {
            string ResponseString;

            List<Message> ReturnArray = new List<Message>();
            WriteMessage("LIST\r\n");
            ResponseString = GetResponse();
            if (!ResponseString.StartsWith("+OK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Pop3Exception(ResponseString);
            }
            bool Done = false;
            while (!Done)
            {
                Regex TempRegex = new Regex(Regex.Escape("+") + "OK.*\r\n");
                if (!ResponseString.EndsWith("\r\n.\r\n"))
                {
                    while (!ResponseString.EndsWith("\r\n.\r\n"))
                        ResponseString += GetResponse();
                }
                ResponseString = TempRegex.Replace(ResponseString, "");
                string[] Seperator = { "\r\n" };
                string[] Values = ResponseString.Split(Seperator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string Value in Values)
                {
                    string[] NewSeperator = { " " };
                    string[] Pair = Value.Split(NewSeperator, StringSplitOptions.RemoveEmptyEntries);
                    if (Pair.Length > 1)
                    {
                        Message TempMessage = new Message();
                        TempMessage.MessageNumber = Int32.Parse(Pair[0]);
                        TempMessage.MessageSize = Int32.Parse(Pair[1]);
                        TempMessage.Retrieved = false;
                        ReturnArray.Add(TempMessage);
                    }
                    else
                    {
                        Done = true;
                        break;
                    }
                }
            }
            return ReturnArray;
        }

        /// <summary>
        /// Gets a specific message from the server
        /// </summary>
        /// <param name="MessageWanted">The message that you want to pull down from the server</param>
        /// <returns>A new message containing the content</returns>
        public Message GetMessage(Message MessageWanted)
        {
            string ResponseString;

            Message TempMessage = new Message();
            TempMessage.MessageSize = MessageWanted.MessageSize;
            TempMessage.MessageNumber = MessageWanted.MessageNumber;

            WriteMessage("RETR " + MessageWanted.MessageNumber + "\r\n");
            ResponseString = GetResponse();
            if (!ResponseString.StartsWith("+OK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Pop3Exception(ResponseString);
            }
            Regex TempRegex = new Regex(Regex.Escape("+") + "OK.*\r\n");
            ResponseString = TempRegex.Replace(ResponseString, "");
            TempRegex = new Regex("\r\n.\r\n$");
            TempMessage.Retrieved = true;
            string BodyText = "";
            while (true)
            {
                if (TempRegex.Match(ResponseString).Success || string.IsNullOrEmpty(ResponseString))
                {
                    BodyText += ResponseString;
                    BodyText = TempRegex.Replace(BodyText, "");
                    break;
                }
                else
                {
                    BodyText += ResponseString;
                }
                ResponseString = GetResponse();
            }
            TempMessage.MessageBody = new Utilities.Web.Email.MIME.MIMEMessage(BodyText);

            return TempMessage;
        }

        /// <summary>
        /// Deletes a message from the server
        /// </summary>
        /// <param name="MessageToDelete">Message to delete</param>
        public void Delete(Message MessageToDelete)
        {
            string ResponseString;
            WriteMessage("DELE " + MessageToDelete.MessageNumber + "\r\n");
            ResponseString = GetResponse();
            if (!ResponseString.StartsWith("+OK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Pop3Exception(ResponseString);
            }
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Writes a message to the server
        /// </summary>
        /// <param name="Message">Information to send to the server</param>
        private void WriteMessage(string Message)
        {
            System.Text.ASCIIEncoding Encoding = new System.Text.ASCIIEncoding();
            byte[] Buffer = new byte[Message.Length];
            Buffer = Encoding.GetBytes(Message);
            if (!UseSSL)
            {
                NetworkStream Stream = GetStream();
                Stream.Write(Buffer, 0, Buffer.Length);
            }
            else
            {
                if (SSLStreamUsing == null)
                {
                    SSLStreamUsing = new SslStream(GetStream());
                    SSLStreamUsing.AuthenticateAsClient(Server);
                }
                SSLStreamUsing.Write(Buffer, 0, Buffer.Length);
            }
        }

        /// <summary>
        /// Gets the response from the server
        /// Note that this uses TCP/IP to get the
        /// messages, which means that the entire message
        /// may not be found in the returned string
        /// (it may only be a partial message)
        /// </summary>
        /// <returns>The response from the server</returns>
        private string GetResponse()
        {
            byte[] Buffer = new byte[1024];
            System.Text.ASCIIEncoding Encoding = new System.Text.ASCIIEncoding();
            string Response = "";
            if (!UseSSL)
            {
                NetworkStream ResponseStream = GetStream();
                while (true)
                {
                    int Bytes = ResponseStream.Read(Buffer, 0, 1024);
                    Response += Encoding.GetString(Buffer, 0, Bytes);
                    if (Bytes != 1024)
                    {
                        break;
                    }
                }
            }
            else
            {
                if (SSLStreamUsing == null)
                {
                    SSLStreamUsing = new SslStream(GetStream());
                    SSLStreamUsing.AuthenticateAsClient(Server);
                }
                while (true)
                {
                    int Bytes = SSLStreamUsing.Read(Buffer, 0, 1024);
                    Response += Encoding.GetString(Buffer, 0, Bytes);
                    if (Bytes != 1024)
                    {
                        break;
                    }
                }
            }
            return Response;
        }

        #endregion

        #region Properties
        private SslStream SSLStreamUsing = null;

        /// <summary>
        /// Decides whether or not we are using
        /// SSL to connect to the server
        /// </summary>
        public bool UseSSL
        {
            get { return _UseSSL; }
            set { _UseSSL = value; }
        }
        private bool _UseSSL;

        /// <summary>
        /// Server location
        /// </summary>
        public string Server
        {
            get { return _Server; }
            set { _Server = value; }
        }
        private string _Server;

        /// <summary>
        /// User name used to log in
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        private string _UserName;

        /// <summary>
        /// Password used to log in
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private string _Password;

        /// <summary>
        /// Port on which to connect
        /// </summary>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        private int _Port;
        #endregion
    }
}