using DatenMeister.DataProvider.CSV;
using DatenMeister.Entities.AsObject.DM;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    public class ExtentLoader
    {
        /// <summary>
        /// Loads all the extents and stores them into the DatenMeister Pool.
        /// If the extent already exists and has the same loading parameters, the 
        /// extent is untouched, otherwise reloaded. 
        /// If the extent does not exist, the extent is loaded by the information.
        /// The information about the loaded extents is stored in the ExtentLoader instance, 
        /// so it needs to be reused. 
        /// </summary>
        /// <param name="extentInformation">Information being used to load the extent</param>
        /// <param name="pool">Pool, where the data is stored</param>
        public void SyncExtents(IReflectiveCollection extentInformation, DatenMeisterPool pool)
        {
            foreach (var item in extentInformation.Select (x=>x.AsIObject()))
            {
                var extentType = ExtentLoadInfo.getExtentType(item);

                IURIExtent loadedExtent = null;
                
                // Checks, if the url is already occupied. 
                var uri = ExtentLoadInfo.getExtentUri(item);
                if (pool.ExtentContainer.Where(x => x.Info.uri == uri).Count() > 0)
                {
                    throw new InvalidOperationException(
                        "An extent with the uri '" + uri + "' is already in database");
                }

                switch (extentType)
                {
                    case "DatenMeister.CSV":
                        var loader = new CSV.CSVDataProvider();
                        var settings = new CSVSettings();
                        settings.HasHeader = false;
                        loadedExtent = loader.Load(CSVExtentLoadInfo.getFilePath(item), settings);
                        break;
                    default:
                        throw new InvalidOperationException("The extent type '" + extentType + "' is unknown.");
                }

                if (loadedExtent != null)
                {
                    pool.Add(loadedExtent, null, Logic.ExtentType.Data);
                }
            }
        }
    }
}
