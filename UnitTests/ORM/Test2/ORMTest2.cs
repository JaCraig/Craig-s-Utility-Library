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
using UnitTests.ORM.Test2.Models;
using Utilities.SQL.SQLServer;
using Utilities.SQL.DataClasses;
using MoonUnit;
using MoonUnit.Attributes;
using System.Data;
using Utilities.SQL.ParameterTypes;

namespace UnitTests.ORM.Test2
{
    public class ORMTest2:IDisposable
    {
        public ORMTest2()
        {
            new Utilities.ORM.ORM(typeof(Task).Assembly);
        }

        [Test]
        public void DatabaseCreation()
        {
            Database DatabaseObject = SQLServer.GetDatabaseStructure("Data Source=localhost;Initial Catalog=ORMTestDatabase2;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(8, DatabaseObject.Tables.Count);
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Project_"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Task_"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Task_Task"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Project_Task"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Project_Audit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Task_Audit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Task_TaskAudit"));
            Assert.True(DatabaseObject.Tables.Exists(x => x.Name == "Project_TaskAudit"));
        }

        [Test]
        public void Save()
        {
            Task TempTask = new Task();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task> Tasks = new List<Task>();
            Task SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            Tasks.Add(SubTask);
            SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project TestProject = new Project();
            TestProject.Description = "This is a test project";
            TestProject.Name = "Test Project";
            List<Task> Tasks2 = new List<Task>();
            Tasks2.Add(TempTask);
            TestProject.Tasks = Tasks2;
            TestProject.Save();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Project_", "Data Source=localhost;Initial Catalog=ORMTestDatabase2;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("This is a test project", Helper.GetParameter<string>("Description_", ""));
                    Assert.Equal("Test Project", Helper.GetParameter<string>("Name_", ""));
                }
                else
                {
                    Assert.Fail("Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Task_", "Data Source=localhost;Initial Catalog=ORMTestDatabase2;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                while (Helper.Read())
                {
                    Assert.Equal("This is a test", Helper.GetParameter<string>("Description_", ""));
                    Assert.Contains(Helper.GetParameter<string>("Name_", ""), new string[] { "Sub task 1", "Sub task 2", "Sub task 3", "Test task" });
                }
            }
        }

        [Test]
        public void Update()
        {
            Task TempTask = new Task();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task> Tasks = new List<Task>();
            Task SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            Tasks.Add(SubTask);
            SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project TestProject = new Project();
            TestProject.Description = "This is a test project";
            TestProject.Name = "Test Project";
            List<Task> Tasks2 = new List<Task>();
            Tasks2.Add(TempTask);
            TestProject.Tasks = Tasks2;
            TestProject.Save();
            TestProject.Description = "Test description2";
            TestProject.Save();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Project_", "Data Source=localhost;Initial Catalog=ORMTestDatabase2;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test description2", Helper.GetParameter<string>("Description_", ""));
                    Assert.Equal("Test Project", Helper.GetParameter<string>("Name_", ""));
                }
                else
                {
                    Assert.Fail("Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM Task_", "Data Source=localhost;Initial Catalog=ORMTestDatabase2;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                while (Helper.Read())
                {
                    Assert.Equal("This is a test", Helper.GetParameter<string>("Description_", ""));
                    Assert.Contains(Helper.GetParameter<string>("Name_", ""), new string[] { "Sub task 1", "Sub task 2", "Sub task 3", "Test task" });
                }
            }
        }

        [Test]
        public void Any()
        {
            Task TempTask = new Task();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task> Tasks = new List<Task>();
            Task SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            Tasks.Add(SubTask);
            SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project TestProject = new Project();
            TestProject.Description = "This is a test project";
            TestProject.Name = "Test Project";
            List<Task> Tasks2 = new List<Task>();
            Tasks2.Add(TempTask);
            TestProject.Tasks = Tasks2;
            TestProject.Save();

            Project TestObject = Project.Any(new EqualParameter<long>(TestProject.ID, "ID_"));
            Assert.Equal(TestProject.Description, TestObject.Description);
            Assert.Equal(TestProject.Name, TestObject.Name);
            Assert.Equal(1, TestObject.Tasks.Count());
            Assert.Equal("Test task", TestObject.Tasks.First().Name);
            Assert.Equal("This is a test", TestObject.Tasks.First().Description);
            foreach (Task TestSubTask in TestObject.Tasks.First().SubTasks)
            {
                Assert.Equal(0, TestSubTask.SubTasks.Count());
                Assert.Contains(TestSubTask.Name, new string[] { "Sub task 1", "Sub task 2", "Sub task 3" });
                Assert.Equal("This is a test", TestSubTask.Description);
                Assert.Equal(new DateTime(1900, 1, 1), TestSubTask.DueDate);
            }
        }

        [Test]
        public void All()
        {
            Task TempTask = new Task();
            TempTask.Description = "This is a test";
            TempTask.DueDate = new DateTime(1900, 1, 1);
            TempTask.Name = "Test task";

            List<Task> Tasks = new List<Task>();
            Task SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 1";
            Tasks.Add(SubTask);
            SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 2";
            Tasks.Add(SubTask);
            SubTask = new Task();
            SubTask.Description = "This is a test";
            SubTask.DueDate = new DateTime(1900, 1, 1);
            SubTask.Name = "Sub task 3";
            Tasks.Add(SubTask);

            TempTask.SubTasks = Tasks;
            Project TestProject = new Project();
            TestProject.Description = "This is a test project";
            TestProject.Name = "Test Project";
            List<Task> Tasks2 = new List<Task>();
            Tasks2.Add(TempTask);
            TestProject.Tasks = Tasks2;
            TestProject.Save();

            IEnumerable<Task> TestObject = Task.All();
            foreach (Task TestTask in TestObject)
            {
                Assert.Contains(TestTask.Name, new string[] { "Sub task 1", "Sub task 2", "Sub task 3", "Test task" });
                Assert.Equal("This is a test", TestTask.Description);
                Assert.Equal(new DateTime(1900, 1, 1), TestTask.DueDate);
            }
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
