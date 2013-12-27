using BurnSystems.FlexBG.Modules.BackgroundWorkerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.BackgroundWorkerM.Logic
{
    public class ExecuteByGameTime : IBackgroundTask
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(ExecuteByGameTime));
 
        /// <summary>
        /// Stores the last tick execution
        /// </summary>
        private Dictionary<long, long> lastTickExecution = new Dictionary<long, long>();

        /// <summary>
        /// Interval between two ticks
        /// </summary>
        private long interval;
        
        /// <summary>
        /// Defines the function that shall be executed when tick is necessary
        /// </summary>
        private Action<IActivates> executionFunction;

        /// <summary>
        /// Initializes a new instance of the ExecuteByGameTime class
        /// </summary>
        /// <param name="interval">Interval in game ticks</param>
        /// <param name="executionFunction">Function that shall be executed. 
        /// Current game will be retrievable in container</param>
        public ExecuteByGameTime(long interval, Action<IActivates> executionFunction)
        {
            Ensure.IsNotNull(executionFunction);

            this.interval = interval;
            this.executionFunction = executionFunction;
        }

        /// <summary>
        /// Gets the predicate for the next tick
        /// </summary>
        /// <param name="container">Container being used</param>
        /// <returns>Next Tick</returns>
        public DateTime GetNextExecutionTime(IActivates container)
        {
            var gameManagement = container.Get<IGameManagement>();
            var clockManagement = container.Get<IGameClockManager>();

            // Get all times
            var minTimeLeft = 60L; // 10 seconds maximum wait time for game
            var games = gameManagement.GetAll();
            foreach (var game in games)
            {
                var currentGameTick = clockManagement.GetTicks(game.Id);
                var lastExecution = this.GetLastExecutionOfGame(game.Id, currentGameTick);
                var nextExecution = lastExecution + this.interval;

                //                                  Time that is left
                minTimeLeft = Math.Min(minTimeLeft, nextExecution - currentGameTick);
            }

            return DateTime.Now.AddSeconds(minTimeLeft);
        }

        public void Execute(ObjectActivation.IActivates container)
        {
            var gameManagement = container.Get<IGameManagement>();
            var clockManagement = container.Get<IGameClockManager>();

            // Get all times
            var games = gameManagement.GetAll();
            foreach (var game in games)
            {
                var currentGameTick = clockManagement.GetTicks(game.Id);
                var lastExecution = this.GetLastExecutionOfGame(game.Id, currentGameTick);
                var nextExecution = lastExecution + this.interval;

                //                Assumed next tick              Time has flown
                //    -8       =    90         +     2    -        100
                var difference = nextExecution - currentGameTick;
                if (difference <= 0)
                {
                    var innerContainer = new ActivationContainer("Background Task Game");
                    innerContainer.BindToName(DeponGamesController.CurrentGameName).ToConstant(game);

                    using (var block = new ActivationBlock("Background Task Game", innerContainer, container as ActivationBlock))
                    {
                        this.executionFunction(block);
                    }

                    this.lastTickExecution[game.Id] = lastExecution + interval;
                }
            }
        }

        private long GetLastExecutionOfGame(long gameId, long currentGameTime)
        {
            long result;
            if (this.lastTickExecution.TryGetValue(gameId, out result))
            {
                return result;
            }
            else
            {
                // Ok, now we have something to do
                // Find the latest game time, where tick has been executed. 
                var latestExecution = ((currentGameTime - 1) / this.interval) * this.interval;
                this.lastTickExecution[gameId] = latestExecution;

                return latestExecution;
            }
        }
    }
}
