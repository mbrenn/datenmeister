using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.GenericObjectStorageM.Interface;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.GenericObjectStorageM
{
    /// <summary>
    /// Implements the IGenericObjectStorage interface
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class GenericObjectStorage : IGenericObjectStorage, IFlexBgRuntimeModule
    {
        /// <summary>
        /// Synchronization object
        /// </summary>
        private object syncObject = new object();

        /// <summary>
        /// Stores the Store
        /// </summary>
        private Store store = new Store();

        /// <summary>
        /// Stores the runtime store
        /// </summary>
        private RuntimeStore runtimeStore = new RuntimeStore();

        /// <summary>
        /// Sets a value into this dynamic storage
        /// </summary>
        /// <typeparam name="T">Type to be used</typeparam>
        /// <param name="path">Path of the value</param>
        /// <param name="value">Value to be stored</param>
        public void Set<T>(string path, T value) where T : class
        {
            lock (this.syncObject)
            {
                // Check, if available
                var entry = this.runtimeStore.Get(typeof(T).FullName, path);
                if (entry != null)
                {
                    entry.Value = value;
                }
                else
                {
                    // Add to store itself
                    entry = new Entry(typeof(T), path, value);

                    this.store.Entries.Add(entry);
                    this.runtimeStore.Add(entry);
                }
            }
        }

        public T Get<T>(string path) where T : class
        {
            lock (this.syncObject)
            {
                var entry = this.runtimeStore.Get(typeof(T).FullName, path);
                if (entry == null)
                {
                    return null;
                }

                return entry.Value as T;
            }
        }

        public void Start()
        {
            lock (this.syncObject)
            {
                this.store = SerializedFile.LoadFromFile<Store>("genericstore", () => new Store());
                this.runtimeStore = new RuntimeStore();

                this.UpdateIndex();
            }
        }

        public void Shutdown()
        {
            lock (this.syncObject)
            {
                SerializedFile.StoreToFile("genericstore", this.store);
            }
        }

        /// <summary>
        /// Updates the index
        /// </summary>
        private void UpdateIndex()
        {
            lock (this.syncObject)
            {
                foreach (var entry in this.store.Entries)
                {
                    this.runtimeStore.Add(entry);   
                }
            }
        }
    }
}
