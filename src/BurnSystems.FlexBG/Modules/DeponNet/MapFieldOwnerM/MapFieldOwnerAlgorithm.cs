using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapFieldOwnerM
{
    /// <summary>
    /// Map field algorithm which determines who is the owner of one field
    /// </summary>
    public abstract class MapFieldOwnerAlgorithm
    {
        /// <summary>
        /// Gets or sets the voxelmap
        /// </summary>
        [Inject(IsMandatory = true)]
        public IVoxelMap VoxelMap
        {
            get;
            set;
        }

        [Inject(ByName = DeponGamesController.CurrentGameName, IsMandatory = true)]
        public Game CurrentGame
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the configuration
        /// </summary>
        public AlgorithmConfig Config
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the method that retrieves all field owners
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<FieldOwner> GetFieldOwners();

        /// <summary>
        /// Executes the algorithm
        /// </summary>
        public virtual void Execute()
        {
            Ensure.That(this.VoxelMap != null, "This class should be instantiated by IActivates");
            var info = this.VoxelMap.GetInfo(this.CurrentGame.Id);
            var fieldOwners = this.GetFieldOwners().ToList();
            for ( var x= 0; x < info.SizeX; x++ )
            {
                for (var y = 0; y < info.SizeY; y++)
                {
                    var fieldOwnerShips = new Dictionary<long, double>();
                    var currentDistance = new ObjectPosition(x,y,0);

                    // For each field, determine strength
                    foreach (var item in fieldOwners)
                    {
                        var distance = ObjectPosition.Distance(
                            item.Position,
                            currentDistance);
                        if (distance >= this.Config.MaxRadius)
                        {
                            continue;
                        }

                        var newInfluence = item.Influence / Math.Pow(1.3, distance);

                        double influence;
                        if (fieldOwnerShips.TryGetValue(item.OwnerId, out influence))
                        {
                            fieldOwnerShips[item.OwnerId] = influence + newInfluence;
                        }
                        else
                        {
                            fieldOwnerShips[item.OwnerId] = influence + newInfluence;
                        }
                    }

                    // Look for highest value
                    if (fieldOwnerShips.Count > 0)
                    {
                        var highest = fieldOwnerShips.OrderByDescending(q => q.Value).First();
                        this.VoxelMap.SetDataByInt64(this.CurrentGame.Id, x, y, this.Config.DataKey, highest.Key);
                    }
                }
            }

        }
    }
}
