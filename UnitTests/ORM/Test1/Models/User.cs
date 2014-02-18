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

using System;
using System.Collections.Generic;
using Utilities.ORM;

namespace UnitTests.ORM.Test1.Models
{
    public class User : ObjectBaseClass<User, long>
    {
        #region Constructor

        public User()
            : base()
        {
            LastLoginDate = new DateTime(2011, 1, 1);
            LastLoginAttempt = new DateTime(2011, 1, 1);
            TemporaryPasswordGenerated = new DateTime(2011, 1, 1);
            FailedLoginAttemptDate = new DateTime(2011, 1, 1);
            LastLockoutDate = new DateTime(2011, 1, 1);
        }

        #endregion

        #region Properties

        public virtual string Email { get; set; }
        public virtual DateTime LastLoginAttempt { get; set; }

        public virtual string Password { get; set; }
        public virtual string PasswordSalt1 { get; set; }
        public virtual string PasswordSalt2 { get; set; }
        public virtual string LastLoginIP { get; set; }
        public virtual bool SoftLocked { get; set; }
        public virtual short FailedLoginCount { get; set; }
        public virtual string TemporaryPassword { get; set; }
        public virtual DateTime LastLoginDate { get; set; }
        public virtual DateTime TemporaryPasswordGenerated { get; set; }
        public virtual DateTime LastPasswordChangedDate { get; set; }
        public virtual DateTime LastLockoutDate { get; set; }
        public virtual DateTime FailedLoginAttemptDate { get; set; }

        public virtual IEnumerable<Role> Roles { get; set; }
        public virtual IEnumerable<Group> Groups { get; set; }
        public virtual Account Account { get; set; }

        #endregion
    }
}
