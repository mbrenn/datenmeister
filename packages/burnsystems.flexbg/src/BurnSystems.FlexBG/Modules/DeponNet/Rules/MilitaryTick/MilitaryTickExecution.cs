using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.MilitaryTick
{
    public class MilitaryTickExecution
    {
        [Inject(ByName = DeponGamesController.CurrentGameName)]
        public Game CurrentGame
        {
            get;
            set;
        }

        /// <summary>
        /// Logger to be used
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(MilitaryTickExecution));

        public static void Execute(IActivates container)
        {
            var watch = new Stopwatch();
            watch.Start();

            var tick = container.Create<MilitaryTickExecution>();
            tick.PerformTick(container);
            watch.Stop();

            logger.LogEntry(LogLevel.Message, "Military Tick: (" + watch.Elapsed.ToString() + ")");
        }

        /// <summary>
        /// Performs the tick
        /// </summary>
        private void PerformTick(IActivates container)
        {

        }
    }
}
