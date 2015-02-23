using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Creates dot net elements
    /// </summary>
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
            Ensure.That(type != null, "type is null");

            var found = this.extent.Mapping.FindByIObjectType(type);
            if (found != null)
            {
                return new DotNetObject(this.extent.Elements(), Activator.CreateInstance(found.DotNetType));
            }
            else
            {
                throw new NotImplementedException("Type is unknown: " + type.ToString());
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
