using BurnSystems.ObjectActivation;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.Entities.FieldInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    public class DatenMeisterPoolExtent : DotNetExtent, IURIExtent
    {
        /// <summary>
        /// Defines the path, where this extent is stored
        /// </summary>
        public const string DefaultUri = "datenmeister:///extents";

        public const string DefaultName = "DatenMeister Extents";

        /// <summary>
        /// Stores the pool
        /// </summary>
        private DatenMeisterPool pool;

        public DatenMeisterPoolExtent(DatenMeisterPool pool)
            : base(DefaultUri)
        {            
            this.pool = pool;
        }

        /// <summary>
        /// Gets the elements as DotNetObject
        /// </summary>
        /// <returns>Enumeration of objects within the extent as dotnet-objects</returns>
        public new IReflectiveSequence Elements()
        {
            return new EnumerationReflectiveSequence<IObject>(this.pool.Instances.Select(
                 x => new DotNetObject(this, x.ToJson(), x.Extent.ContextURI())));
        }

        public static void AddView(IURIExtent viewExtent)
        {
            var factory = Factory.GetFor(viewExtent);
            // Creates the view for the extents
            var extentViewObj = factory.CreateInExtent(viewExtent, DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
            var asObjectExtentview = new DatenMeister.Entities.AsObject.FieldInfo.TableView(extentViewObj);

            asObjectExtentview.setExtentUri(DatenMeisterPoolExtent.DefaultUri);
            asObjectExtentview.setAllowDelete(false);
            asObjectExtentview.setAllowEdit(false);
            asObjectExtentview.setAllowNew(false);
            asObjectExtentview.setName("Extents");
            asObjectExtentview.setFieldInfos(new DotNetSequence(
                new TextField("Name", "name"),
                new TextField("URI", "uri"),
                new TextField("Type", "type"),
                new TextField("Filename", "filename")));
        }
    }
}
