using Ironman.Core.API.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntegrationTests.Controllers
{
    public class APITestController : APIControllerBaseClass
    {
        protected override int Version { get { return 1; } }
    }
}