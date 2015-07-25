using Ironman.Core.API.Manager.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.ORM;
using Utilities.ORM.BaseClasses;
using Utilities.ORM.Interfaces;

namespace IntegrationTests.Models
{
    public enum TestEnum
    {
        Value1 = 0,
        Value2,
        Value3
    }

    public class TestClass : ObjectBaseClass<TestClass, long>
    {
        public virtual bool BoolReference { get; set; }

        public virtual byte[] ByteArrayReference { get; set; }

        public virtual byte ByteReference { get; set; }

        public virtual char CharReference { get; set; }

        public virtual decimal DecimalReference { get; set; }

        public virtual double DoubleReference { get; set; }

        public virtual TestEnum EnumReference { get; set; }

        public virtual float FloatReference { get; set; }

        public virtual Guid GuidReference { get; set; }

        public virtual int IntReference { get; set; }

        public virtual long LongReference { get; set; }

        public virtual IEnumerable<TestClass> ManyToManyIEnumerable { get; set; }

        public virtual List<TestClass> ManyToManyList { get; set; }

        public virtual IEnumerable<TestClass> ManyToOneIEnumerable { get; set; }

        public virtual TestClass ManyToOneItem { get; set; }

        public virtual List<TestClass> ManyToOneList { get; set; }

        public virtual TestClass Map { get; set; }

        public virtual string NullStringReference { get; set; }

        public virtual short ShortReference { get; set; }

        public virtual string StringReference { get; set; }
    }

    public class TestClassAPIMapping : APIMappingBaseClass<TestClass, long>
    {
        public TestClassAPIMapping()
            : base()
        {
            ID(x => x.ID);
            Reference(x => x.Active);
            Reference(x => x.BoolReference);
            Reference(x => x.ByteArrayReference);
            Reference(x => x.ByteReference);
            Reference(x => x.CharReference);
            Reference(x => x.DecimalReference);
            Reference(x => x.DoubleReference);
            Reference(x => x.EnumReference);
            Reference(x => x.FloatReference);
            Reference(x => x.GuidReference);
            Reference(x => x.IntReference);
            Reference(x => x.LongReference);
            Reference(x => x.NullStringReference);
            Reference(x => x.ShortReference);
            Reference(x => x.StringReference);
            Reference(x => x.DateCreated);
            Reference(x => x.DateModified);
            MapList(x => x.ManyToManyIEnumerable);
            MapList(x => x.ManyToManyList);
            MapList(x => x.ManyToOneIEnumerable);
            MapList(x => x.ManyToOneList);
            Map(x => x.ManyToOneItem);
            Map(x => x.Map);
        }
    }

    public class TestClassDatabase : IDatabase
    {
        public bool Audit
        {
            get { return false; }
        }

        public string Name
        {
            get { return "Data Source=localhost;Initial Catalog=IntegrationTestDatabase;Integrated Security=SSPI;Pooling=false"; }
        }

        public int Order
        {
            get { return 0; }
        }

        public bool Readable
        {
            get { return true; }
        }

        public bool Update
        {
            get { return true; }
        }

        public bool Writable
        {
            get { return true; }
        }
    }

    public class TestClassMapping : MappingBaseClass<TestClass, TestClassDatabase>
    {
        public TestClassMapping()
            : base()
        {
            ID(x => x.ID).SetAutoIncrement();
            ManyToMany(x => x.ManyToManyIEnumerable).SetTableName("ManyToManyIEnumerable").SetCascade();
            ManyToMany(x => x.ManyToManyList).SetTableName("ManyToManyList").SetCascade();
            ManyToOne(x => x.ManyToOneIEnumerable).SetTableName("ManyToOneIEnumerable").SetCascade();
            ManyToOne(x => x.ManyToOneList).SetTableName("ManyToOneList").SetCascade();
            ManyToOne(x => x.ManyToOneItem).SetTableName("ManyToOneList").SetCascade();
            Map(x => x.Map).SetCascade();
            Reference(x => x.BoolReference);
            Reference(x => x.ByteArrayReference).SetMaxLength(100);
            Reference(x => x.ByteReference);
            Reference(x => x.CharReference);
            Reference(x => x.DecimalReference).SetMaxLength(8);
            Reference(x => x.DoubleReference);
            Reference(x => x.EnumReference);
            Reference(x => x.FloatReference);
            Reference(x => x.GuidReference);
            Reference(x => x.IntReference);
            Reference(x => x.LongReference);
            Reference(x => x.NullStringReference).SetMaxLength(100);
            Reference(x => x.ShortReference);
            Reference(x => x.StringReference).SetMaxLength(100);
            Reference(x => x.DateCreated);
            Reference(x => x.DateModified);
            Reference(x => x.Active);
        }
    }
}