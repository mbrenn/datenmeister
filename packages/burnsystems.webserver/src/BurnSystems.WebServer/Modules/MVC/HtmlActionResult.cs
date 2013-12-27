using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    public class HtmlActionResult : TextActionResult
    {
        public HtmlActionResult(string htmlText)
            : base(htmlText, "text/html; charset=UTF-8")
        {
        }
    }
}
