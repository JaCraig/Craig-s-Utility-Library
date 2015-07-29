using IntegrationTests.Models;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace IntegrationTests
{
    public class MvcApplication : Ironman.Core.BaseClasses.HttpApplicationBase
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            if (TestClass.Any() != null)
                return;
            for (int x = 0; x < 5; ++x)
            {
                var TempObject = new TestClass();
                TempObject.BoolReference = true;
                TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                TempObject.ByteReference = 12;
                TempObject.CharReference = 'v';
                TempObject.DecimalReference = 1.4213m;
                TempObject.DoubleReference = 1.32645d;
                TempObject.EnumReference = TestEnum.Value2;
                TempObject.FloatReference = 1234.5f;
                TempObject.GuidReference = Guid.NewGuid();
                TempObject.IntReference = 145145;
                TempObject.LongReference = 763421;
                //TempObject.ManyToManyIEnumerable = new TestClass[] { new TestClass(), new TestClass() };
                //TempObject.ManyToManyList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
                //TempObject.ManyToOneIEnumerable = new TestClass[] { new TestClass(), new TestClass(), new TestClass() };
                //TempObject.ManyToOneItem = new TestClass();
                //TempObject.ManyToOneList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
                //TempObject.Map = new TestClass();
                TempObject.NullStringReference = null;
                TempObject.ShortReference = 5423;
                TempObject.StringReference = "agsdpghasdg";
                TempObject.Save();
            }
        }
    }
}