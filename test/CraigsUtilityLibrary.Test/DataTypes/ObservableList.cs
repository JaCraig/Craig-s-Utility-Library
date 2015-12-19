using System;
using System.Collections.Specialized;
using Xunit;

namespace UnitTests.DataTypes
{
    public class ObservableList
    {
        [Fact]
        public void Add()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.Add(10);
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Add, Value2);
        }

        [Fact]
        public void AddRange()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.AddRange(new int[] { 10, 11, 12, 13 });
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Add, Value2);
        }

        [Fact]
        public void Clear()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.Clear();
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Reset, Value2);
        }

        [Fact]
        public void Insert()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.Insert(0, 1);
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Add, Value2);
        }

        [Fact]
        public void InsertRange()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.InsertRange(0, new int[] { 1, 2, 3, 4, 5 });
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Add, Value2);
        }

        [Fact]
        public void Remove()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.Add(10);
            ListVariable.Remove(0);
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Remove, Value2);
        }

        [Fact]
        public void RemoveAll()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.Add(10);
            ListVariable.RemoveAll(x => x > 0);
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Remove, Value2);
        }

        [Fact]
        public void RemoveAt()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.Add(10);
            ListVariable.RemoveAt(0);
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Remove, Value2);
        }

        [Fact]
        public void RemoveRange()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.Add(10);
            ListVariable.RemoveRange(0, 1);
            Assert.Equal("Count", Value);
            Assert.Equal(NotifyCollectionChangedAction.Remove, Value2);
        }

        [Fact]
        public void IndexSet()
        {
            string Value = "";
            NotifyCollectionChangedAction Value2 = NotifyCollectionChangedAction.Move;
            var ListVariable = new Utilities.DataTypes.ObservableList<int>();
            ListVariable.PropertyChanged += (x, y) => Value = y.PropertyName;
            ListVariable.CollectionChanged += (x, y) => Value2 = y.Action;
            ListVariable.Add(10);
            Value = "";
            ListVariable[0] = 11;
            Assert.Equal("", Value);
            Assert.Equal(NotifyCollectionChangedAction.Replace, Value2);
        }
    }
}