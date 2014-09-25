using BurnSystems.Test;
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

        /// <summary>
        /// Gets or sets a flag whether the extent is currently dirty
        /// That means, it has unsaved changes
        /// </summary>
        public bool IsDirty
        {
            get;
            set;
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

        public IReflectiveSequence Elements()
        {
            return new CSVExtentReflectiveSequence(this);
        }

        /// <summary>
        /// Defines the reflective sequence being used 
        /// </summary>
        public class CSVExtentReflectiveSequence : ListReflectiveSequence<IObject>
        {
            public CSVExtent extent;

            public CSVExtentReflectiveSequence(CSVExtent extent)
                : base(extent)
            {
                Ensure.That(extent != null);
                this.extent = extent;
            }

            protected override IList<IObject> GetList()
            {
                return this.extent.Objects;
            }

            public override void OnChange()
            {
                this.extent.IsDirty = true;
                base.OnChange();
            }

            public override IObject ConvertInstanceToInternal(object value)
            {
                if (value is CSVObject)
                {
                    var valueAsCSV = value as CSVObject;
                    return value as CSVObject;
                }
                else
                {
                    lock (this.extent.objects)
                    {
                        return new CSVObject(this.extent, null);
                    }
                }
            }
        }
    }
}
