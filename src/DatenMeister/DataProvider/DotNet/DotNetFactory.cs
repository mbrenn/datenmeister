using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.DotNet
{
    public class DotNetFactory : IFactory
    {
        private DotNetExtent extent;

        public DotNetFactory(DotNetExtent extent)
        {
            Ensure.That(extent != null);
            this.extent = extent;
        }

        public IObject create(IObject type)
        {
            var found = this.extent.Mapping.FindByIObjectType(type);
            if (found != null)
            {
                return new DotNetObject(this.extent, Activator.CreateInstance(found.DotNetType));
            }
            else
            {
                return null;
            }
        }

        public IObject createFromString(IObject dataType, string value)
        {
            throw new NotImplementedException();
        }

        public string convertToString(IObject dataType, IObject value)
        {
            throw new NotImplementedException();
        }
    }
}
