using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Manages the mapping between .Net Types and DatenMeister types
    /// </summary>
    public class DotNetTypeMapping : IMapsMetaClassFromDotNet
    {
        /// <summary>
        /// Stores the mappings
        /// </summary>
        private List<DotNetTypeInformation> mappings = new List<DotNetTypeInformation>();

        public DotNetTypeInformation Add(Type dotNetType, IObject type)
        {
            var information = new DotNetTypeInformation()
            {
                DotNetType = dotNetType,
                Type = type
            };

            this.mappings.Add(information);

            return information;
        }

        public DotNetTypeInformation FindByDotNetType(Type type)
        {
            return this.mappings.Where(x => x.DotNetType == type).FirstOrDefault();
        }

        public DotNetTypeInformation FindByIObjectType(IObject type)
        {
            return this.mappings.Where(x => x.Type == type).FirstOrDefault();
        }

        /// <summary>
        /// Removes the mapping for a certain type
        /// </summary>
        /// <param name="type">Type, whose mapping shall get removed</param>
        public void RemoveFor(Type type)
        {
            this.mappings.RemoveAll(x => x.DotNetType == type);
        }

        public IObject GetMetaClass(Type dotNetType)
        {
            var result = this.FindByDotNetType(dotNetType);
            if (result != null)
            {
                return result.Type;
            }

            return null;
        }

        public Type GetDotNetType(IObject type)
        {
            var result = this.FindByIObjectType(type);
            if (result != null)
            {
                return result.DotNetType;
            }

            return null;
        }
    }
}
