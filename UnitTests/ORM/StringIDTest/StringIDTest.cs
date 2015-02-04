/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Data;
using System.Linq;
using UnitTests.ORM.ListTest.Models;
using UnitTests.ORM.Test2.Models;
using Utilities.ORM.ExtensionMethods;
using Utilities.SQL.DataClasses;
using Utilities.SQL.ParameterTypes;
using Utilities.SQL.SQLServer;
using Xunit;

namespace UnitTests.ORM.ListTest
{
    public class StringIDTest:IDisposable
    {
        public StringIDTest()
        {
            new Utilities.ORM.ORM(false, typeof(Task).Assembly);
        }

        [Fact]
        public void DatabaseCreation()
        {
            Database DatabaseObject = SQLServer.GetDatabaseStructure("Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(16, DatabaseObject.Tables.Count);
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Project3_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Task3_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Task3_Task3"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Project3_Task3"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Project3_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Task3_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Task3_Task3Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Project3_Task3Audit"));
        }

        [Fact]
        public void Save()
        {
            Task3 TempTask = new Task3();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task3> Tasks = new List<Task3>();
            Task3 SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);
            SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project3 TestProject = new Project3();
            TestProject.ID = "A";
            TestProject.Name = "Test Project";
            List<Task3> Tasks3 = new List<Task3>();
            Tasks3.Add(TempTask);
            TestProject.Tasks = Tasks3;
            TestProject.Save();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Project3_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("", Helper.GetParameter<string>("Description_", ""));
                    Assert.Equal("Test Project", Helper.GetParameter<string>("Name_", ""));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Task3_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                while (Helper.Read())
                {
                    Assert.Equal("This is a test", Helper.GetParameter<string>("Description_", ""));
                    Assert.Contains(Helper.GetParameter<string>("Name_", ""), new string[] { "Sub task 1", "Sub task 3", "Sub task 3", "Test task" });
                }
            }
        }

        [Fact]
        public void Update()
        {
            Task3 TempTask = new Task3();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task3> Tasks = new List<Task3>();
            Task3 SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);
            SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project3 TestProject = new Project3();
            TestProject.ID = "A";
            TestProject.Name = "Test Project";
            List<Task3> Tasks3 = new List<Task3>();
            Tasks3.Add(TempTask);
            TestProject.Tasks = Tasks3;
            TestProject.Save();
            TestProject.Name = "Test description3";
            TestProject.Save();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Project3_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("", Helper.GetParameter<string>("Description_", ""));
                    Assert.Equal("Test description3", Helper.GetParameter<string>("Name_", ""));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Task3_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                while (Helper.Read())
                {
                    Assert.Equal("This is a test", Helper.GetParameter<string>("Description_", ""));
                    Assert.Contains(Helper.GetParameter<string>("Name_", ""), new string[] { "Sub task 1", "Sub task 3", "Sub task 3", "Test task" });
                }
            }
        }

        [Fact]
        public void Any()
        {
            Task3 TempTask = new Task3();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task3> Tasks = new List<Task3>();
            Task3 SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);
            SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project3 TestProject = new Project3();
            TestProject.ID = "A";
            TestProject.Name = "Test Project";
            List<Task3> Tasks3 = new List<Task3>();
            Tasks3.Add(TempTask);
            TestProject.Tasks = Tasks3;
            TestProject.Save();

            Project3 TestObject = Project3.Any(new StringEqualParameter(TestProject.ID, "ID_", 100));
            Assert.Equal(TestProject.ID, TestObject.ID);
            Assert.Equal(TestProject.Name, TestObject.Name);
            Assert.Equal(1, TestObject.Tasks.Count());
            Assert.Equal("Test task", TestObject.Tasks.First().Name);
            Assert.Equal("This is a test", TestObject.Tasks.First().Description);
            foreach (Task3 TestSubTask in TestObject.Tasks.First().SubTasks)
            {
                Assert.Equal(0, TestSubTask.SubTasks.Count());
                Assert.Contains(TestSubTask.Name, new string[] { "Sub task 1", "Sub task 3", "Sub task 3" });
                Assert.Equal("This is a test", TestSubTask.Description);
                Assert.Equal(new DateTime(1900, 1, 1), TestSubTask.DueDate);
            }
        }

        [Fact]
        public void All()
        {
            Task3 TempTask = new Task3();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task3> Tasks = new List<Task3>();
            Task3 SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);
            SubTask = new Task3();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project3 TestProject = new Project3();
            TestProject.ID = "A";
            TestProject.Name = "Test Project";
            List<Task3> Tasks3 = new List<Task3>();
            Tasks3.Add(TempTask);
            TestProject.Tasks = Tasks3;
            TestProject.Save();

            IEnumerable<Task3> TestObject = Task3.All();
            foreach (Task3 TestTask in TestObject)
            {
                Assert.Contains(TestTask.Name, new string[] { "Sub task 1", "Sub task 3", "Sub task 3", "Test task" });
                Assert.Equal("This is a test", TestTask.Description);
                Assert.Equal(new DateTime(1900, 1, 1), TestTask.DueDate);
            }
        }

        [Fact]
        public void Paged()
        {
            for (int x = 0; x < 100; ++x)
            {
                Task3 TempTask = new Task3();
                TempTask.Description = "This is a test";
                TempTask.DueDate = new DateTime(1900, 1, 1);
                TempTask.Name = "Test task";
                TempTask.Save();
            }

            IEnumerable<Task3> TestObject = Task3.Paged();
            Assert.Equal(25, TestObject.Count());
            foreach (Task3 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 1, 25);
            }

            TestObject = Task3.Paged(CurrentPage: 1);
            Assert.Equal(25, TestObject.Count());
            foreach (Task3 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 26, 50);
            }

            TestObject = Task3.Paged(CurrentPage: 2);
            Assert.Equal(25, TestObject.Count());
            foreach (Task3 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 51, 75);
            }

            TestObject = Task3.Paged(CurrentPage: 3);
            Assert.Equal(25, TestObject.Count());
            foreach (Task3 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 76, 100);
            }

            Assert.Equal(4, Task3.PageCount());
        }

        [Fact]
        public void Paged3()
        {
            for (int x = 0; x < 100; ++x)
            {
                Task3 TempTask = new Task3();
                TempTask.Description = "This is a test";
                TempTask.DueDate = new DateTime(1900, 1, 1);
                TempTask.Name = "Test task";
                TempTask.Save();
            }

            IEnumerable<Task3> TestObject = Task3.PagedCommand("SELECT * FROM Task3_ WHERE ID_>@ID", "", 25, 0, new EqualParameter<long>(50, "ID"));
            Assert.Equal(25, TestObject.Count());
            foreach (Task3 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 51, 75);
            }

            TestObject = Task3.PagedCommand("SELECT * FROM Task3_ WHERE ID_>@ID", "", 25, 1, new EqualParameter<long>(50, "ID"));
            Assert.Equal(25, TestObject.Count());
            foreach (Task3 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 76, 100);
            }

            TestObject = Task3.PagedCommand("SELECT * FROM Task3_ WHERE ID_>@ID", "", 25, 2, new EqualParameter<long>(50, "ID"));
            Assert.Equal(0, TestObject.Count());

            Assert.Equal(2, Task3.PageCount("SELECT * FROM Task3_ WHERE ID_>@ID", 25, new EqualParameter<long>(50, "ID")));
        }

        public void Dispose()
        {
            Utilities.ORM.ORM.Destroy();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("",  CommandType.Text, "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.Batch().AddCommand("ALTER DATABASE ORMTestDatabase3 SET OFFLINE WITH ROLLBACK IMMEDIATE", CommandType.Text)
                    .AddCommand("ALTER DATABASE ORMTestDatabase3 SET ONLINE", CommandType.Text)
                    .AddCommand("DROP DATABASE ORMTestDatabase3", CommandType.Text);
                Helper.ExecuteNonQuery();
            }
        }
    }
}
