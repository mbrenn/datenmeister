using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.CSV
{
    public class CSVFactory : IFactory
    {
        private CSVExtent extent;

        public CSVFactory(CSVExtent extent)
        {
            Ensure.That(extent != null);
            this.extent = extent;
        }

        public IObject create(IObject type)
        {
            return new CSVObject(this.extent, null);
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
