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
using System.Linq;
using System.Text;
using MoonUnit.Attributes;
using MoonUnit;
using Utilities.SQL.SQLServer;
using Utilities.SQL.DataClasses;
using Utilities.DataTypes.ExtensionMethods;
using System.Data;
using UnitTests.ORM.Test1.Models;
using Utilities.SQL.ParameterTypes;

namespace UnitTests.ORM.Test1
{
    public class ORMTest1:IDisposable
    {
        public ORMTest1()
        {
            new Utilities.ORM.ORM(typeof(Account).Assembly);
        }

        [Test]
        public void DatabaseCreation()
        {
            Database DatabaseObject = SQLServer.GetDatabaseStructure("Data Source=localhost;Initial Catalog=ORMTestDatabase;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(18, DatabaseObject.Tables.Count);
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Account_"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Group_"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Role_"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "User_"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Account_Audit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Group_Audit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Role_Audit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "User_Audit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Account_User"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Account_UserAudit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Group_User"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Group_UserAudit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Role_User"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Role_UserAudit"));
        }

        [Test]
        public void ManyToOneTest()
        {
            Item Temp = new Item();
            Temp.Name = "Parent";
            for (int x = 0; x < 5; ++x)
            {
                Item Child = new Item();
                Child.Name = "Child "+x.ToString();
                Child.Parent=Temp;
                Temp.Children.Add(Child);
            }
            Temp.Save();
            Item Parent = Item.Any(new EqualParameter<long>(Temp.ID, "ID_"));
            foreach (Item Child in Parent.Children)
                Assert.Equal(Parent, Child.Parent);
            Assert.Equal(5, Parent.Children.Count);
            Assert.Equal(null, Parent.Parent);
            Parent.Children = Parent.Children.Remove(x => x.Name == "Child 0").ToList();
            Parent.Save();
            Parent = Item.Any(new EqualParameter<long>(Temp.ID, "ID_"));
            foreach (Item Child in Parent.Children)
                Assert.Equal(Parent, Child.Parent);
            Assert.Equal(4, Parent.Children.Count);
            Assert.Equal(null, Parent.Parent);
            Parent.Delete();
            Parent = Item.Any(new EqualParameter<long>(Temp.ID, "ID_"));
            Assert.Null(Parent);
            Assert.Equal(1, Item.All().Count());
        }

        public void Dispose()
        {
            Utilities.ORM.ORM.Destroy();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("ALTER DATABASE ORMTestDatabase2 SET OFFLINE WITH ROLLBACK IMMEDIATE", "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();
                Helper.Command = "ALTER DATABASE ORMTestDatabase2 SET ONLINE";
                Helper.ExecuteNonQuery();
                Helper.Command = "DROP DATABASE ORMTestDatabase2";
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("ALTER DATABASE ORMTestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE", "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();
                Helper.Command = "ALTER DATABASE ORMTestDatabase SET ONLINE";
                Helper.ExecuteNonQuery();
                Helper.Command = "DROP DATABASE ORMTestDatabase";
                Helper.ExecuteNonQuery();
            }
        }
    }
}
