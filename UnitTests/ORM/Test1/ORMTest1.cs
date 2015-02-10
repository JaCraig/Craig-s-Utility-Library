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

using System.Linq;
using UnitTests.ORM.Test1.Models;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.SQL.DataClasses;
using Utilities.SQL.ParameterTypes;
using Utilities.SQL.SQLServer;
using Xunit;

namespace UnitTests.ORM.Test1
{
    public class ORMTest1 : DatabaseBaseClass
    {
        [Fact]
        public void DatabaseCreation()
        {
            Database DatabaseObject = SQLServer.GetDatabaseStructure("Data Source=localhost;Initial Catalog=ORMTestDatabase;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(20, DatabaseObject.Tables.Count);
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Account_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Group_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Role_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "User_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Office_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Office_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Account_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Group_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Role_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "User_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Account_User"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Account_UserAudit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Group_User"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Group_UserAudit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Role_User"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Role_UserAudit"));
        }

        [Fact]
        public void ManyToOneTest()
        {
            Item Temp = new Item();
            Temp.Name = "Parent";
            for (int x = 0; x < 5; ++x)
            {
                Item Child = new Item();
                Child.Name = "Child " + x.ToString();
                Child.Parent = Temp;
                Child.Value = EnumValue.Value3;
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
            Assert.Equal(EnumValue.Value3, Parent.Children[0].Value);
            Assert.Equal(EnumValue.Value3, Parent.Children[1].Value);
            Assert.Equal(EnumValue.Value3, Parent.Children[2].Value);
            Assert.Equal(EnumValue.Value3, Parent.Children[3].Value);
            Parent.Delete();
            Parent = Item.Any(new EqualParameter<long>(Temp.ID, "ID_"));
            Assert.Null(Parent);
            Assert.Equal(1, Item.All().Count());
        }

        [Fact]
        public void MapTest()
        {
            Office Temp = new Office() { Name = "Test Office" };
            Temp.User = new User() { Email = "something@something.com" };
            Temp.User2 = new User() { Email = "something2@something.com" };
            Temp.Save();
            Assert.Equal(1, Office.All().Count());
            Temp = Office.Any();
            Assert.Equal("Test Office", Temp.Name);
            Assert.Equal("something@something.com", Temp.User.Email);
            Assert.Equal("something2@something.com", Temp.User2.Email);
            Temp.Save();
            Temp.Name = "Test Office2";
            Temp.User.Email = "something2@something.com";
            Temp.User2.Email = "something3@something.com";
            Temp.Save();
            Assert.Equal(1, Office.All().Count());
            Temp = Office.Any();
            Assert.Equal("Test Office2", Temp.Name);
            Assert.Equal("something2@something.com", Temp.User.Email);
            Assert.Equal("something3@something.com", Temp.User2.Email);
        }
    }
}