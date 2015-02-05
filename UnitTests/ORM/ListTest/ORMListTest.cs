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
using System.Data;
using System.Linq;
using UnitTests.ORM.ListTest.Models;
using Utilities.ORM.ExtensionMethods;
using Utilities.SQL.DataClasses;
using Utilities.SQL.ParameterTypes;
using Utilities.SQL.SQLServer;
using Xunit;

namespace UnitTests.ORM.ListTest
{
    public class ORMListTest : DatabaseBaseClass
    {
        [Fact]
        public void All()
        {
            Task2 TempTask = new Task2();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task2> Tasks = new List<Task2>();
            Task2 SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            Tasks.Add(SubTask);
            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project2 TestProject = new Project2();
            TestProject.Description = "This is a test project";
            TestProject.Name = "Test Project";
            List<Task2> Tasks2 = new List<Task2>();
            Tasks2.Add(TempTask);
            TestProject.Tasks = Tasks2;
            TestProject.Save();

            IEnumerable<Task2> TestObject = Task2.All();
            foreach (Task2 TestTask in TestObject)
            {
                Assert.Contains(TestTask.Name, new string[] { "Sub task 1", "Sub task 2", "Sub task 3", "Test task" });
                Assert.Equal("This is a test", TestTask.Description);
                Assert.Equal(new DateTime(1900, 1, 1), TestTask.DueDate);
            }
        }

        [Fact]
        public void Any()
        {
            Task2 TempTask = new Task2();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task2> Tasks = new List<Task2>();
            Task2 SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            Tasks.Add(SubTask);
            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project2 TestProject = new Project2();
            TestProject.Description = "This is a test project";
            TestProject.Name = "Test Project";
            List<Task2> Tasks2 = new List<Task2>();
            Tasks2.Add(TempTask);
            TestProject.Tasks = Tasks2;
            TestProject.Save();

            Project2 TestObject = Project2.Any(new EqualParameter<long>(TestProject.ID, "ID_"));
            Assert.Equal(TestProject.Description, TestObject.Description);
            Assert.Equal(TestProject.Name, TestObject.Name);
            Assert.Equal(1, TestObject.Tasks.Count());
            Assert.Equal("Test task", TestObject.Tasks.First().Name);
            Assert.Equal("This is a test", TestObject.Tasks.First().Description);
            foreach (Task2 TestSubTask in TestObject.Tasks.First().SubTasks)
            {
                Assert.Equal(0, TestSubTask.SubTasks.Count());
                Assert.Contains(TestSubTask.Name, new string[] { "Sub task 1", "Sub task 2", "Sub task 3" });
                Assert.Equal("This is a test", TestSubTask.Description);
                Assert.Equal(new DateTime(1900, 1, 1), TestSubTask.DueDate);
            }
        }

        [Fact]
        public void DatabaseCreation()
        {
            Database DatabaseObject = SQLServer.GetDatabaseStructure("Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(16, DatabaseObject.Tables.Count);
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Project2_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Task2_"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Task2_Task2"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Project2_Task2"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Project2_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Task2_Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Task2_Task2Audit"));
            Assert.True(DatabaseObject.Tables.Any(x => x.Name == "Project2_Task2Audit"));
        }

        [Fact]
        public void Paged()
        {
            for (int x = 0; x < 100; ++x)
            {
                Task2 TempTask = new Task2();
                TempTask.Description = "This is a test";
                TempTask.DueDate = new DateTime(1900, 1, 1);
                TempTask.Name = "Test task";
                TempTask.Save();
            }

            IEnumerable<Task2> TestObject = Task2.Paged();
            Assert.Equal(25, TestObject.Count());
            foreach (Task2 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 1, 25);
            }

            TestObject = Task2.Paged(CurrentPage: 1);
            Assert.Equal(25, TestObject.Count());
            foreach (Task2 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 26, 50);
            }

            TestObject = Task2.Paged(CurrentPage: 2);
            Assert.Equal(25, TestObject.Count());
            foreach (Task2 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 51, 75);
            }

            TestObject = Task2.Paged(CurrentPage: 3);
            Assert.Equal(25, TestObject.Count());
            foreach (Task2 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 76, 100);
            }

            Assert.Equal(4, Task2.PageCount());
        }

        [Fact]
        public void Paged2()
        {
            for (int x = 0; x < 100; ++x)
            {
                Task2 TempTask = new Task2();
                TempTask.Description = "This is a test";
                TempTask.DueDate = new DateTime(1900, 1, 1);
                TempTask.Name = "Test task";
                TempTask.Save();
            }

            IEnumerable<Task2> TestObject = Task2.PagedCommand("SELECT * FROM Task2_ WHERE ID_>@ID", "", 25, 0, new EqualParameter<long>(50, "ID"));
            Assert.Equal(25, TestObject.Count());
            foreach (Task2 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 51, 75);
            }

            TestObject = Task2.PagedCommand("SELECT * FROM Task2_ WHERE ID_>@ID", "", 25, 1, new EqualParameter<long>(50, "ID"));
            Assert.Equal(25, TestObject.Count());
            foreach (Task2 TestTask in TestObject)
            {
                Assert.InRange(TestTask.ID, 76, 100);
            }

            TestObject = Task2.PagedCommand("SELECT * FROM Task2_ WHERE ID_>@ID", "", 25, 2, new EqualParameter<long>(50, "ID"));
            Assert.Equal(0, TestObject.Count());

            Assert.Equal(2, Task2.PageCount("SELECT * FROM Task2_ WHERE ID_>@ID", 25, new EqualParameter<long>(50, "ID")));
        }

        [Fact]
        public void Save()
        {
            Task2 TempTask = new Task2();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task2> Tasks = new List<Task2>();
            Task2 SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            Tasks.Add(SubTask);
            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project2 TestProject = new Project2();
            TestProject.Description = "This is a test project";
            TestProject.Name = "Test Project";
            List<Task2> Tasks2 = new List<Task2>();
            Tasks2.Add(TempTask);
            TestProject.Tasks = Tasks2;
            TestProject.Save();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Project2_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("This is a test project", Helper.GetParameter<string>("Description_", ""));
                    Assert.Equal("Test Project", Helper.GetParameter<string>("Name_", ""));
                }
                else
                {
                    Assert.False(true, "Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Task2_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                while (Helper.Read())
                {
                    Assert.Equal("This is a test", Helper.GetParameter<string>("Description_", ""));
                    Assert.Contains(Helper.GetParameter<string>("Name_", ""), new string[] { "Sub task 1", "Sub task 2", "Sub task 3", "Test task" });
                }
            }
        }

        [Fact]
        public void SaveIEnumerableExtension()
        {
            List<Task2> Tasks = new List<Task2>();
            for (int x = 0; x < 100; ++x)
            {
                Task2 TempTask = new Task2();
                TempTask.Description = "This is a test";
                TempTask.DueDate = new DateTime(1900, 1, 1);
                TempTask.Name = "Test task";
                Tasks.Add(TempTask);
            }
            Tasks.Save<Task2, long>();

            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Task2_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                int Counter = 0;
                while (Helper.Read())
                {
                    Assert.Equal("This is a test", Helper.GetParameter<string>("Description_", ""));
                    Assert.Equal("Test task", Helper.GetParameter<string>("Name_", ""));
                    ++Counter;
                }
                Assert.Equal(100, Counter);
            }
        }

        [Fact]
        public void Update()
        {
            Task2 TempTask = new Task2();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task2> Tasks = new List<Task2>();
            Task2 SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            Tasks.Add(SubTask);
            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project2 TestProject = new Project2();
            TestProject.Description = "This is a test project";
            TestProject.Name = "Test Project";
            List<Task2> Tasks2 = new List<Task2>();
            Tasks2.Add(TempTask);
            TestProject.Tasks = Tasks2;
            TestProject.Save();
            TestProject.Description = "Test description2";
            TestProject.Save();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Project2_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test description2", Helper.GetParameter<string>("Description_", ""));
                    Assert.Equal("Test Project", Helper.GetParameter<string>("Name_", ""));
                }
                else
                {
                    Assert.False(true, "Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Task2_", CommandType.Text, "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.ExecuteReader();
                while (Helper.Read())
                {
                    Assert.Equal("This is a test", Helper.GetParameter<string>("Description_", ""));
                    Assert.Contains(Helper.GetParameter<string>("Name_", ""), new string[] { "Sub task 1", "Sub task 2", "Sub task 3", "Test task" });
                }
            }
        }

        [Fact]
        public void Update2()
        {
            Task2 TempTask = new Task2();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task2> Tasks = new List<Task2>();
            Task2 SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);

            SubTask = new Task2();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            SubTask.Save();
            TempTask.Save();

            TempTask = Task2.Any(new EqualParameter<long>(TempTask.ID, "ID_"));
            Tasks = new List<Task2>();
            Tasks.Add(SubTask);
            TempTask.SubTasks = Tasks;
            TempTask.Save();
            TempTask = Task2.Any(new EqualParameter<long>(TempTask.ID, "ID_"));
            Assert.True(TempTask.SubTasks.Any(x => x.ID == SubTask.ID));
        }
    }
}