using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.WebServer.Modules.MVC;

namespace BurnSystems.WebServer.UnitTests.Controller
{
    /// <summary>
    /// Calculator for webmethods
    /// </summary>
    public class CalcController : Modules.MVC.Controller
    {
        public class SumData
        {
            public double Summand1
            {
                get;
                set;
            }

            public double Summand2
            {
                get;
                set;
            }
        }

        [WebMethod()]
        [IfMethodIs("post")]
        public IActionResult Sum([PostModel] SumData sum)
        {
            return this.Json(new 
            {
                Sum = sum.Summand1 + sum.Summand2
            });
        }
    }
}
