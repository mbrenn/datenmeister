using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.IdGeneratorM
{
    /// <summary>
    /// Implements the id generator
    /// </summary>
    public class IdGenerator : IIdGenerator
    {
        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [Inject]
        public IdStatusDb Db
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the next id
        /// </summary>
        /// <param name="entityType">Requested entity type</param>
        /// <returns></returns>
        public long NextId(int entityType)
        {
            Ensure.That(this.Db != null);

            lock (this.Db.Status.Ids)
            {
                long result;
                if (this.Db.Status.Ids.TryGetValue(entityType, out result))
                {
                    this.Db.Status.Ids[entityType] = result + 1;
                    return result + 1;
                }
                else
                {
                    this.Db.Status.Ids[entityType] = 1;
                    return 1;
                }
            }
        }

        /// <summary>
        /// Integrates the default configuration into container
        /// </summary>
        /// <param name="container">Container, where this stuff shall be added</param>
        public static void Integrate(ActivationContainer container)
        {
            var db = new IdStatusDb();
            container.Bind<IFlexBgRuntimeModule>().ToConstant(db);
            container.Bind<IdStatusDb>().ToConstant(db);
            container.Bind<IIdGenerator>().To<IdGenerator>();
        }
    }
}
