using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.MapFieldOwnerM;
using BurnSystems.FlexBG.Modules.LockMasterM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.EconomyTick
{
    public class EconomyTickExecution
    {
        [Inject(ByName = DeponGamesController.CurrentGameName)]
        public Game CurrentGame
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public ILockMaster LockMaster
        {
            get;
            set;
        }

        /// <summary>
        /// Logger to be used
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(EconomyTickExecution));

        public static void Execute(IActivates container)
        {
            var watch = new Stopwatch();
            watch.Start();

            var tick = container.Create<EconomyTickExecution>();
            tick.PerformTick(container);
            watch.Stop();

            logger.LogEntry(LogLevel.Message, "Economy Tick: (" + watch.Elapsed.ToString() + ")");
        }

        /// <summary>
        /// Performs the tick
        /// </summary>
        private void PerformTick(IActivates container)
        {
            using (this.LockMaster.AcquireWriteLock(EntityType.Game, this.CurrentGame.Id))
            {
                var algo = container.Create<FieldOwnershipByBuilding>();
                algo.Config = GameConfig.Rules.EmpireRules;
                algo.Execute();
            }
        }
    }
}
