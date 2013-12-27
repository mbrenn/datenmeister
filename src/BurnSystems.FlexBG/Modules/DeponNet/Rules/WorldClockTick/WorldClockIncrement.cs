using BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.WorldClockTick
{
    /// <summary>
    /// Increments the worldclock of all active games by one time increment
    /// </summary>
    public class WorldClockIncrement
    {
        private ILog logger = new ClassLogger(typeof(WorldClockIncrement));
        [Inject]
        public IGameManagement GameManagement
        {
            get;
            set;
        }

        [Inject]
        public IGameClockManager ClockManager
        {
            get;
            set;
        }

        public static void Increment(IActivates activates)
        {
            var tthis = activates.Create<WorldClockIncrement>();
            tthis.Execute();
        }

        /// <summary>
        /// Executes the world clock incrementation
        /// </summary>
        private void Execute()
        {
            foreach (var game in this.GameManagement.GetAll())
            {
                if (game.IsPaused)
                {
                    // logger.LogEntry(LogLevel.Verbose, "Game is paused: " + game.Title);
                    continue;
                }

                // logger.LogEntry(LogLevel.Verbose, "Increment Worldclock for game: " + game.Title);
                this.ClockManager.IncrementTicks(game.Id, 1);
            }
        }
    }
}
