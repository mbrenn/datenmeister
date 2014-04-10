using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.CSV
{
    public class CSVExtent : IURIExtent
    {
        /// <summary>
        /// Gets or sets the pool, where the object is stored
        /// </summary>
        public IPool Pool
        {
            get;
            set;
        }

        private string contextUri;

        private List<string> headerNames = new List<string>();

        private List<IObject> objects = new List<IObject>();

        public CSVSettings Settings
        {
            get;
            set;
        }

        public List<IObject> Objects
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

        public IObject CreateObject(IObject type)
        {
            lock (this.objects)
            {
                var nr = this.objects.Count;
                var element = new CSVObject(nr, this, null);

                this.objects.Add(element);
                return element;
            }
        }

        public void RemoveObject(IObject element)
        {
            lock (this.Objects)
            {
                this.Objects.Remove(element);
            }
        }
    }
}
