﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.Logic;
using DatenMeister.Pool;
using BurnSystems.ObjectActivation;
using DatenMeister.DataProvider;
using DatenMeister.Logic.TypeResolver;

namespace DatenMeister
{
    /// <summary>
    /// Defines the datenmeister pool
    /// </summary>
    public class DatenMeisterPool : IPool
    {
        /// <summary>
        /// Stores the syncronisation object
        /// </summary>
        private object syncObject = new object();

        /// <summary>
        /// Stores the extents itself
        /// </summary>
        private List<ExtentInstance> extents = new List<ExtentInstance>();

        /// <summary>
        /// Gets the extents as a read copty
        /// </summary>
        public IEnumerable<IURIExtent> Extents
        {
            get
            {
                lock (this.syncObject)
                {
                    return this.extents.Select(x => x.Extent).ToList();
                }
            }
        }

        /// <summary>
        /// Gets a list of instances, this list is thread-safe
        /// </summary>
        public IEnumerable<ExtentInstance> Instances
        {
            get
            {
                lock (this.syncObject)
                {
                    return this.extents.ToList();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the DatenMeisterPool.
        /// Use Create to create a new instance
        /// </summary>
        private DatenMeisterPool()
        {
        }

        /// <summary>
        /// Adds the uri extent to datenmeister pool
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        public void Add(IURIExtent extent, string storagePath, ExtentType extentType)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(extent);
                this.Add(extent, storagePath, null, extentType);
            }
        }

        /// <summary>
        /// Adds the an extent to the datenmeister pool and defines
        /// the storage path and a name to the given pool
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        /// <param name="storagePath">Path, where pool is stored</param>
        /// <param name="name">Name of the pool</param>
        public void Add(IURIExtent extent, string storagePath, string name, ExtentType extentType)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(extent);
                this.Add(
                    new ExtentInstance(extent, storagePath, name, extentType));
            }
        }

        public void Add(ExtentInstance instance)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(instance.Extent);

                // Check, if an extent with the same url already exists
                var number = 0;
                foreach (var extent in this.extents)
                {
                    if (extent.Extent.ContextURI() == instance.Extent.ContextURI())
                    {
                        this.extents.RemoveAt(number);
                        break;
                    }

                    number++;
                }

                // Adds the instance and assign it
                this.extents.Add(instance);
                instance.Extent.Pool = this;
            }
        }

        private void CheckIfExtentAlreadyInAnyPool(IURIExtent extent)
        {
            if (extent.Pool != null)
            {
                throw new InvalidOperationException("The extent is already assigned to a pool. An extent cannot be assigned to two pools at the same time");
            }
        }

        /// <summary>
        /// Gets or sets the pool being used for the application
        /// </summary>
        private static DatenMeisterPool ApplicationPool
        {
            get;
            set;
        }

        /// <summary>
        /// Performs the default binding, attached to the current pool.
        /// </summary>
        private static void DoDefaultBinding()
        {
            // At the moment, reset the complete Binding
            Global.Reset();

            // Initializes the default factory provider
            Global.Application.Bind<IFactoryProvider>().To<FactoryProvider>();

            // Initializes the default pool
            Global.Application.Bind<IPool>().ToConstant(ApplicationPool);
            Global.Application.Bind<DatenMeisterPool>().ToConstant(ApplicationPool);

            // Initializes the default resolver
            Global.Application.Bind<IPoolResolver>().To(x => new PoolResolver() { Pool = ApplicationPool });

            // Initializes the default type resolver
            Global.Application.Bind<ITypeResolver>().To<TypeResolverImpl>();
        }

        /// <summary>
        /// Creates a default empty pool, where the DatenMeisterPoolExtent is already associated.
        /// The Pool will also be bound to the Global Application Binding
        /// </summary>
        /// <returns>The created pool</returns>
        public static DatenMeisterPool Create()
        {
            ApplicationPool = new DatenMeisterPool();
            DoDefaultBinding();

            // Adds the extent for the extents
            var poolExtent = new DatenMeisterPoolExtent(ApplicationPool);
            ApplicationPool.Add(poolExtent, null, DatenMeisterPoolExtent.DefaultName, ExtentType.Extents);

            return ApplicationPool;
        }
    }
}
