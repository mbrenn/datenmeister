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
        /// Executes the mapping of keys to the values
        /// </summary>
        /// <param name="storage">Storage to be used</param>
        public abstract void ExecuteMapping(GroupByDictionary storage);

        /// <summary>
        /// Returns the elements of the reflective sequence. 
        /// </summary>
        /// <returns></returns>
        public override IReflectiveSequence Elements()
        {
            var list = new List<GroupByObject>();
            var storage = new GroupByDictionary();

            this.ExecuteMapping(storage);

            foreach (var pair in storage.Storage)
            {
                list.Add(new GroupByObject(
                    this.source,
                    pair.Key,
                    new ListWrapperReflectiveSequence<object>(this, pair.Value)));
            }

            return new GroupByReflectiveSequence(this.source, list);
        }

        public IEnumerable<GroupByObject> ElementsAsGroupBy()
        {
            foreach (var element in this.Elements())
            {
                yield return element.AsSingle() as GroupByObject;
            }
        }
    }
}
