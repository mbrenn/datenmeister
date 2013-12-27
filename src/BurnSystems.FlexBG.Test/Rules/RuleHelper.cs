using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM;
using BurnSystems.FlexBG.Modules.UserM.Controllers;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*namespace BurnSystems.FlexBG.Test.Rules
{
    public class RuleHelper : IDisposable
    {
        private Runtime runtime;

        public ActivationContainer Container
        {
            get;
            set;
        }

        public long GameId
        {
            get;
            set;
        }

        public RuleHelper()
        {
            Log.TheLog.Reset();
            Log.TheLog.AddLogProvider(new DebugProvider());
            Log.TheLog.FilterLevel = LogLevel.Verbose;

            SerializedFile.ClearCompleteDataDirectory();

            // Creates the game
            var container = new ActivationContainer("Game");
            DeponNet2.Init.CreateGameLogic(container, false);

            this.runtime = new Runtime(container, "config");

            this.runtime.StartUpCore();

            // Creates game
            var gameManagement = container.Get<IGameManagement>();
            var controller = container.Create<DeponGamesAdminController>();
            this.GameId = controller.CreateGame(
                new Modules.DeponNet.GameM.Admin.DeponCreateGameModel()
                {
                    Title = "Test",
                    Description = "Description",
                    MaxPlayers = 10,
                    MapHeight = 200,
                    MapWidth = 200
                });

            this.Container = new ActivationContainer("Inner", container);
            this.Container.BindToName(DeponGamesController.CurrentGameName).ToConstant(gameManagement.Get(this.GameId));
        }

        /// <summary>
        /// Creates the user for testing purposes
        /// </summary>
        /// <returns>Id of the user</returns>
        public long CreateUser()
        {
            var userManagement = this.Container.Get<IUserManagement>();
            var user = new User();
            user.HasAgreedToTOS = true;
            user.Username = "Test";
            userManagement.SetPassword(user, "Password");
            user.EMail = "test@depon.net";

            return userManagement.AddUser(user);
        }

        public long CreatePlayer(long userId)
        {
            var playerRules = this.Container.Get<IPlayerRulesLogic>();

            return playerRules.CreatePlayer(
                new PlayerCreationParams()
                {
                    GameId = this.GameId,
                    UserId = userId,
                    Playername = "Testplayer",
                    Empirename = "Testempire",
                    FirstTownName = "Testtown"
                });
        }

        public void Dispose()
        {
            this.runtime.ShutdownCore();
        }
    }
}
*/