using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class LocalBuildingDatabase : IFlexBgRuntimeModule
    {
        /// <summary>
        /// Stores the synchronization
        /// </summary>
        private object syncObject = new object();

        public BuildingsData BuildingsStore
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sync object
        /// </summary>
        public object SyncObject
        {
            get { return this.syncObject; }
        }

        /// <summary>
        /// Called, when started
        /// </summary>
        public void Start()
        {
            this.BuildingsStore = SerializedFile.LoadFromFile<BuildingsData>("buildings.db", () => new BuildingsData());
        }

        /// <summary>
        /// Called when shutdowned
        /// </summary>
        public void Shutdown()
        {
            SerializedFile.StoreToFile("buildings.db", this.BuildingsStore);
        }
    }
}
