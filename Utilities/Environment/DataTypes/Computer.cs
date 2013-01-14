/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
#endregion

namespace Utilities.Environment.DataTypes
{
    /// <summary>
    /// Represents a computer
    /// </summary>
    public class Computer
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Computer Name</param>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        public Computer(string Name, string UserName = "", string Password = "")
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            this.Name = Name;
            this.UserName = UserName;
            this.Password = Password;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Computer Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// BIOS info
        /// </summary>
        public virtual BIOS BIOS
        {
            get
            {
                if (BIOS_ == null)
                {
                    BIOS_ = new BIOS(Name, UserName, Password);
                }
                return BIOS_;
            }
        }

        /// <summary>
        /// BIOS info
        /// </summary>
        protected BIOS BIOS_ { get; set; }

        /// <summary>
        /// Application info
        /// </summary>
        public virtual Applications Applications
        {
            get
            {
                if (Applications_ == null)
                {
                    Applications_ = new Applications(Name, UserName, Password);
                }
                return Applications_;
            }
        }

        /// <summary>
        /// Applications
        /// </summary>
        protected Applications Applications_ { get; set; }

        /// <summary>
        /// Network info
        /// </summary>
        public virtual Network Network
        {
            get
            {
                if (Network_ == null)
                {
                    Network_ = new Network(Name, UserName, Password);
                }
                return Network_;
            }
        }

        /// <summary>
        /// Network info
        /// </summary>
        protected Network Network_ { get; set; }

        /// <summary>
        /// Operating system info
        /// </summary>
        public virtual OperatingSystem OperatingSystem
        {
            get
            {
                if (OperatingSystem_ == null)
                {
                    OperatingSystem_ = new OperatingSystem(Name, UserName, Password);
                }
                return OperatingSystem_;
            }
        }

        /// <summary>
        /// Operating system info
        /// </summary>
        protected OperatingSystem OperatingSystem_ { get; set; }

        /// <summary>
        /// Holds a list of users that have logged into the machine recently
        /// </summary>
        public virtual User LatestUsers
        {
            get
            {
                if (User_ == null)
                {
                    User_ = new User(Name, UserName, Password);
                }
                return User_;
            }
        }

        /// <summary>
        /// User info
        /// </summary>
        protected User User_ { get; set; }


        #endregion
    }
}