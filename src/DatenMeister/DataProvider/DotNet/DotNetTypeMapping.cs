using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Manages the mapping between .Net Types and DatenMeister types
    /// </summary>
    public class DotNetTypeMapping
    {
        /// <summary>
        /// Stores the mappings
        /// </summary>
        private List<DotNetTypeInformation> mappings = new List<DotNetTypeInformation>();

        public void Add(Type dotNetType, IObject type)
        {
            var information = new DotNetTypeInformation()
            {
                DotNetType = dotNetType,
                Type = type
            };

            this.mappings.Add(information);
        }

        public DotNetTypeInformation FindByDotNetType(Type type)
        {
            return this.mappings.Where(x => x.DotNetType == type).FirstOrDefault();
        }

        public DotNetTypeInformation FindByIObjectType(IObject type)
        {
            return this.mappings.Where(x => x.Type == type).FirstOrDefault();
        }
    }
}
