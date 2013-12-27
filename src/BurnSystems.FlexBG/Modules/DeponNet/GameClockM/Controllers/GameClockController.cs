using BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Controllers
{
    /// <summary>
    /// Defines the gameclock
    /// </summary>
    public class GameClockController : Controller
    {
        [Inject(ByName = DeponGamesController.CurrentGameName)]
        public Game CurrentGame
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IGameClockManager GameClock
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult GetTime()
        {
            var ticks = this.GameClock.GetTicks(this.CurrentGame.Id);
            var result = new
            {
                success = true,
                ticks = ticks,
                isPaused = this.CurrentGame.IsPaused
            };

            return this.Json(result);
        }
    }
}