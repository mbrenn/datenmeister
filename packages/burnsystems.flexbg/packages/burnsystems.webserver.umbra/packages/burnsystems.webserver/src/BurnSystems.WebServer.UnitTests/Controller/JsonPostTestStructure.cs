using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.UnitTests.Controller
{
    public class JsonPostTestStructure
    {
        public string prop
        {
            get;
            set;
        }

        public int numberProp
        {
            get;
            set;
        }

        public List<int> arrayProp
        {
            get;
            set;
        }

        public class Sub
        {
            public string name
            {
                get;
                set;
            }

            public string prename
            {
                get;
                set;
            }
        }

        public Sub substructure
        {
            get;
            set;
        }
    }
}
