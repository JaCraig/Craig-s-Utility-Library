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
using UnitTests.ORM.Test1.Config;
using Utilities.ORM.Mapping;

namespace UnitTests.ORM.Test1.Models.Mappings
{
    public class UserMapping : Mapping<User, Test1Config>
    {
        public UserMapping()
        {
            Reference(x => x.Active).SetDefaultValue(() => true);
            Reference(x => x.DateCreated).SetDefaultValue(() => DateTime.Now);
            Reference(x => x.DateModified).SetDefaultValue(() => DateTime.Now);
            Reference(x => x.Email).SetMaxLength(256);
            Reference(x => x.FailedLoginCount).SetDefaultValue(() => 0);
            ID(x => x.ID).TurnOnAutoIncrement();
            Reference(x => x.LastLoginAttempt).SetDefaultValue(() => new DateTime(2011, 1, 1));
            Reference(x => x.LastLoginDate).SetDefaultValue(() => new DateTime(2011, 1, 1));
            Reference(x => x.LastLoginIP).SetMaxLength(45);
            Reference(x => x.Password).SetMaxLength(32);
            Reference(x => x.PasswordSalt1).SetMaxLength(32);
            Reference(x => x.PasswordSalt2).SetMaxLength(32);
            Reference(x => x.SoftLocked).SetDefaultValue(() => false);
            Reference(x => x.TemporaryPassword).SetMaxLength(32);
            Reference(x => x.TemporaryPasswordGenerated).SetDefaultValue(() => new DateTime(2011, 1, 1));
            Reference(x => x.LastPasswordChangedDate).SetDefaultValue(() => new DateTime(2011, 1, 1));
            Reference(x => x.LastLockoutDate).SetDefaultValue(() => new DateTime(2011, 1, 1));
            Reference(x => x.FailedLoginAttemptDate).SetDefaultValue(() => new DateTime(2011, 1, 1));

            ManyToMany(x => x.Roles);
            ManyToMany(x => x.Groups);
            ManyToOne(x => x.Account);
        }
    }
}
