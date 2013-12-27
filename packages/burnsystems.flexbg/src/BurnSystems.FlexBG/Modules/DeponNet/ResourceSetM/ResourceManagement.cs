using BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.MapM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM.Interface;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    public class ResourceManagement : IResourceManagement
    {
        private  static ILog logger = new ClassLogger(typeof(ResourceManagement));

        /// <summary>
        /// Gets or sets the database
        /// </summary>
        [Inject(IsMandatory = true)]
        public LocalResourceDatabase Database
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IResourceTypeProvider ResourceTypeProvider
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IGameClockManager GameClockManager
        {
            get;
            set;
        }

        [Inject(IsMandatory = true, ByName = DeponGamesController.CurrentGameName)]
        public Game CurrentGame
        {
            get;
            set;
        }

        public ResourceSetBag GetResources(int entityType, long entityId)
        {
            bool isNew;
            var resourceBag = this.Database.Find(entityType, entityId, out isNew);
            if (isNew)
            {
                // Resourcebag has been recently created, so set the ticks of last update
                resourceBag.TicksOfLastUpdate = this.GameClockManager.GetTicks(this.CurrentGame.Id);
            }

            this.UpdateResources(resourceBag);
            return resourceBag;
        }

        public void SetAvailable(int entityType, long entityId, ResourceSet resources)
        {
            bool isNew; // Not required, since ticks of last update will always be updated
            var resourceBag = this.Database.Find(entityType, entityId, out isNew);

            // No update is necessary. Amont is directly set
            resourceBag.Available.Set(resources);
            resourceBag.TicksOfLastUpdate = this.GameClockManager.GetTicks(this.CurrentGame.Id);
        }

        /// <summary>
        /// Updates the available resources by adding the change to the maximum. 
        /// </summary>
        /// <param name="resourceBag">Resource Bag which shall be updated</param>
        private void UpdateResources(ResourceSetBag resourceBag)
        {
            lock (resourceBag)
            {
                var gameTicks = this.GameClockManager.GetTicks(this.CurrentGame.Id);

                var difference = gameTicks - resourceBag.TicksOfLastUpdate;

                var current = resourceBag.Change.CloneScale(difference);
                current.Add(resourceBag.Available);
                if (resourceBag.Maximum != null)
                {
                    current.ApplyMaximum(resourceBag.Maximum, false);
                }

                resourceBag.Available.Set(current);
                resourceBag.TicksOfLastUpdate = gameTicks;
            }
        }

        /// <summary>
        /// Converts a resource set as json 
        /// </summary>
        /// <returns></returns>
        public object AsJson(ResourceSetBag resourceSetBag)
        {
            var result = new Dictionary<string, object>();
            
            foreach (var pair in resourceSetBag.Available.Resources)
            {
                var fieldType = this.ResourceTypeProvider.Get(pair.Key);
                if (fieldType != null)
                {
                    result[fieldType.Token] =
                        new
                        {
                            available = pair.Value,
                            change = resourceSetBag.Change[pair.Key]
                        };
                }
                else
                {
                    logger.Message(
                        string.Format(
                            "Unknown fieldtype with number: {0}",
                            pair.Key));
                }
            }

            return result;
        }
    }
}