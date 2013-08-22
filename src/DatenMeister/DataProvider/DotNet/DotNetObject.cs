using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    class DotNetObject : IObject
    {
        /// <summary>
        /// Stores the id
        /// </summary>
        private Guid id = Guid.NewGuid();

        private object value;

        public DotNetObject(object value)
        {
            this.value = value;
        }
        
        public object Get(string propertyName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ObjectPropertyPair> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Set(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        public void Unset(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the id of the dotnet object
        /// </summary>
        public string Id
        {
            get { return this.id.ToString(); }
        }
    }
}
