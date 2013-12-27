using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class LocalResourceDatabase : IFlexBgRuntimeModule
    {
        /// <summary>
        /// Stores the synchronization
        /// </summary>
        private object syncObject = new object();

        public ResourcesData ResourceStore
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
            this.ResourceStore = SerializedFile.LoadFromFile<ResourcesData>("resources.db", () => new ResourcesData());
        }

        /// <summary>
        /// Called when shutdowned
        /// </summary>
        public void Shutdown()
        {
            SerializedFile.StoreToFile("resources.db", this.ResourceStore);
        }

        /// <summary>
        /// Finds a specific resourcesetbag
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="entityId">ID of the entity</param>
        /// <returns>Found bag</returns>
        public ResourceSetBag Find(int entityType, long entityId, out bool isNew)
        {
            lock (this.syncObject)
            {
                var found = this.ResourceStore.Resources.Where(x => x.EntityType == entityType && x.EntityId == entityId)
                    .FirstOrDefault();

                if (found == null)
                {
                    found = new ResourceSetBag(entityType, entityId);
                    found.TicksOfLastUpdate = -1;
                    this.ResourceStore.Resources.Add(found);

                    isNew = true;
                }
                else
                {
                    isNew = false;
                }

                return found;
            }
        }
    }
}