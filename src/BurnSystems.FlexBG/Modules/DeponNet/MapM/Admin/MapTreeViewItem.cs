using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapM.Admin
{
    /// <summary>
    /// Defines the map tree view item
    /// </summary>
    public class MapTreeViewItem : BaseTreeViewItem
    {
        /// <summary>
        /// Gets or sets the instance id
        /// </summary>
        public long InstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the map info
        /// </summary>
        public VoxelMapInfo MapInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the entity
        /// </summary>
        public override object Entity
        {
            get
            {
                return this.MapInfo;
            }
        }

        /// <summary>
        /// Gets the title
        /// </summary>
        public override string Title
        {
            get
            {
                return "Map";
            }
        }

        /// <summary>
        /// Initializes a new instance of the MapTreeViewItem class
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <param name="instanceId">Id of the instance</param>
        public MapTreeViewItem(IActivates container, long instanceId)
        {
            this.InstanceId = instanceId;
            this.Id = instanceId;

            var voxelMap = container.Get<IVoxelMap>();
            this.MapInfo = voxelMap.GetInfo(instanceId);
        }
    }
}
