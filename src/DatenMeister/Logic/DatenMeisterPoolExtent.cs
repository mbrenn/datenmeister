using BurnSystems.ObjectActivation;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.Entities.FieldInfos;
using DatenMeister.Logic.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// This class abstracts the databasepool to an extent, so it can be used via default interfaces. 
    /// </summary>
    public class DatenMeisterPoolExtent : DotNetExtent, IURIExtent
    {
        /// <summary>
        /// Defines the path, where this extent is stored
        /// </summary>
        public const string DefaultUri = "datenmeister:///extents";

        /// <summary>
        /// Defines the default name of the Extent, containing all extents
        /// </summary>
        public const string DefaultName = "DatenMeister Extents";

        /// <summary>
        /// Stores the pool
        /// </summary>
        private DatenMeisterPool pool;
        
        /// <summary>
        /// Initializes a new view of the DatenMeisterPoolExtent class
        /// </summary>
        /// <param name="pool">Pool to be used for this extent</param>
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
            return new EnumerationReflectiveSequence<IObject>(this,
                this.pool.ExtentContainer.Select(
                    x => new DotNetObject(this.Elements(), x.Info, x.Extent.ContextURI())));
        }

        /// <summary>
        /// Adds the view for a list of all extents. 
        /// </summary>
        /// <param name="viewExtent">Extent, containing a list of the table views</param>
        public static IObject AddView(IURIExtent viewExtent)
        {
            var factory = Factory.GetFor(viewExtent);
            // Creates the view for the extents
            var extentViewObj = factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
            var asObjectExtentview = new DatenMeister.Entities.AsObject.FieldInfo.TableView(extentViewObj);

            asObjectExtentview.setExtentUri(DatenMeisterPoolExtent.DefaultUri);
            asObjectExtentview.setAllowDelete(false);
            asObjectExtentview.setAllowEdit(false);
            asObjectExtentview.setAllowNew(false);
            asObjectExtentview.setName("Extents");
            asObjectExtentview.setFieldInfos(new DotNetSequence(
                ViewHelper.ViewTypes,
                new TextField("Name", "name"),
                new TextField("ExtentType", "extentType"),
                new TextField("URI", "uri")
                {
                    width = 150
                },
                new TextField("Type", "extentClass")
                {
                    width = 150
                },
                new TextField("Filename", "storagePath")));

            viewExtent.Elements().add(extentViewObj);

            return extentViewObj;
        }
    }
}
