using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.CSV
{
    public class CSVExtent : IURIExtent
    {
        private string contextUri;

        private List<string> headerNames = new List<string>();

        private List<CSVObject> objects = new List<CSVObject>();

        public CSVSettings Settings
        {
            get;
            set;
        }

        public List<CSVObject> Objects
        {
            get { return this.objects; }
        }

        public List<string> HeaderNames
        {
            get { return this.headerNames; }
        }

        public CSVExtent(string uri, CSVSettings settings)
        {
            this.Settings = settings;
            this.contextUri = uri;
        }

        public string ContextURI()
        {
            return this.contextUri;
        }

        public IEnumerable<IObject> Elements()
        {
            return this.Objects;
        }

        public void StoreChanges()
        {
            throw new NotImplementedException();
        }
    }
}
