using DatenMeister.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations.GroupBy
{
    public abstract class BaseGroupByTransformation : BaseTransformation
    {
        /// <summary>
        /// Initializes a new instance of the BaseGroupByTransformation
        /// </summary>
        /// <param name="collection"></param>
        public BaseGroupByTransformation(IReflectiveCollection collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Executes the mapping of keys to the values
        /// </summary>
        /// <param name="storage">Storage to be used</param>
        public abstract void ExecuteMapping(GroupByDictionary storage);

        /// <summary>
        /// Returns the elements of the reflective sequence. 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<object> getAll()
        {
            var list = new List<GroupByObject>();
            var storage = new GroupByDictionary();

            this.ExecuteMapping(storage);

            foreach (var pair in storage.Storage)
            {
                yield return new GroupByObject(
                    this.source.Extent,
                    pair.Key,
                    new ListWrapperReflectiveSequence<object>(this.Extent, pair.Value));
            }
        }

        public IEnumerable<GroupByObject> ElementsAsGroupBy()
        {
            foreach (var element in this)
            {
                yield return element.AsSingle() as GroupByObject;
            }
        }
    }
}
