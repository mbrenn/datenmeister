using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.MVC;
using BurnSystems.WebServer.Modules.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers
{
    /// <summary>
    /// Controller for games
    /// </summary>
    public class DeponGamesController : Controller
    {
        [Inject(IsMandatory=true)]
        public IGameManagement GameManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IUserManagement UserManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true, ByName="CurrentUser")]
        public User CurrentUser
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public Session Session
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the activation container
        /// </summary>
        [Inject(IsMandatory = true)]
        public ActivationBlock ActivationBlock
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current game, reqired for checking if user is in current game
        /// </summary>
        [Inject(ByName = DeponGamesController.CurrentGameName)]
        public Game CurrentGame
        {
            get;
            set;
        }

        /// <summary>
        /// Session variable for current game
        /// </summary>
        public const string CurrentGameName = "FlexBG.CurrentGame";

        /// <summary>
        /// Lists all games
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public IActionResult GetGames()
        {
            var games = this.GameManagement.GetAll();
            var players = this.PlayerManagement.GetPlayersOfUser(this.CurrentUser.Id);

            var result = new
            {
                success = true,
                games = games.Select(x => new
                {
                    id = x.Id,
                    isPaused = x.IsPaused,
                    title = x.Title,
                    playerCount = this.PlayerManagement.GetPlayersOfGame(x.Id).Count(),
                    isInGame = players.Any(y => y.GameId == x.Id)
                })
            };

            return this.Json(result);
        }

        [WebMethod]
        public IActionResult JoinGame([PostModel] JoinGameModel model)
        {
            using (var block = CreateActivationBlockInGameScope(this.ActivationBlock, this.GameManagement, model.GameId))
            {
                var playerRules = block.Get<IPlayerRulesLogic>();

                var playerId = playerRules.CreatePlayer(
                    new PlayerCreationParams()
                    {
                        Playername = model.Playername,
                        Empirename = model.Empirename,
                        GameId = model.GameId,
                        FirstTownName = model.Townname,
                        UserId = this.CurrentUser.Id
                    });

                var result = new
                {
                    success = true,
                    playerId = playerId
                };

                return this.Json(result);
            }
        }

        [WebMethod]
        public IActionResult ContinueGame([PostModel] ContinueGameModel model)
        {
            using (var block = CreateActivationBlockInGameScope(this.ActivationBlock, this.GameManagement, model.GameId))
            {
                var playerRules = block.Get<IPlayerRulesLogic>();

                if (!playerRules.CanUserContinueGame(this.CurrentUser.Id, model.GameId))
                {
                    throw new MVCProcessException("continuegame_playernotingame", "Player cannot join game.");
                }
                else
                {
                    // Ok, we are in game, now add a cookie for game (Cookies are for games, mjam)
                    this.Session["FlexBG.CurrentGame"] = model.GameId;

                    return this.SuccessJson();
                }
            }
        }

        [WebMethod]
        public IActionResult GetGameInfo()
        {
            var result = new
            {
                success = true,
                title = this.CurrentGame.Title,
                maxPlayers = this.CurrentGame.MaxPlayers,
                isPaused = this.CurrentGame.IsPaused,
                id = this.CurrentGame.Id,
                description = this.CurrentGame.Description
            };

            return this.Json(result);
        }

        /// <summary>
        /// Leaves the gameplay. This is the counter method to <c>ContinueGame</c>. 
        /// </summary>
        /// <returns>Result of the web request</returns>
        [WebMethod]
        public IActionResult LeaveGame()
        {
            this.Session.Remove("FlexBG.CurrentGame");

            return this.SuccessJson();
        }

        /// <summary>
        /// Checks, if player is in game
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public IActionResult HasUserJoined()
        {
            if (this.CurrentGame == null
                || this.PlayerManagement.GetPlayersOfUser(this.CurrentUser.Id).All(x => x.GameId != this.CurrentGame.Id))
            {
                return this.Json(
                    new
                    {
                        success = true,
                        isInGame = false
                    });
            }
            else
            {
                return this.Json(
                    new
                    {
                        success = true,
                        isInGame = true,
                        gameId = this.CurrentGame.Id
                    });
            }
        }

        /// <summary>
        /// Static helper method which retrieves the current game out of the session variables
        /// which have been associated to the user
        /// </summary>
        /// <param name="activates">Activation container</param>
        /// <returns>Found game or null, if user has not continued a game</returns>
        public static Game GetGameOfWebRequest(IActivates activates)
        {
            var session = activates.Get<Session>();
            var gameManagement = activates.Get<IGameManagement>();
            Ensure.That(session != null, "Binding to Session has not been done");
            Ensure.That(gameManagement != null, "Binding to IGameManagement has not been done");

            var gameIdObj = session["FlexBG.CurrentGame"];
            if (gameIdObj == null || !(gameIdObj is long))
            {
                // Nothing found
                return null;
            }

            return gameManagement.Get((long)gameIdObj);
        }

        /// <summary>
        /// Creates an ActivationBlock, where the CurrentGame has been set with the given id. 
        /// </summary>
        /// <param name="block">Activationblock to be used</param>
        /// <param name="gameManagement">Game Management being used to retrieve the information</param>
        /// <param name="gameId">Id of the game</param>
        /// <returns>Inner activation which shall be disposed by the caller</returns>
        public static ActivationBlock CreateActivationBlockInGameScope(ActivationBlock block, IGameManagement gameManagement, long gameId)
        {
            var container = new ActivationContainer("GameScope");
            container.BindToName(DeponGamesController.CurrentGameName)
                .ToConstant(gameManagement.Get(gameId));

            return new ActivationBlock("GameScope Block", container, block);
        }

        /// <summary>
        /// Creates an ActivationBlock, where the CurrentGame has been set with the given id. 
        /// </summary>
        /// <param name="block">Activationblock to be used</param>
        /// <param name="gameId">Id of the game</param>
        /// <returns>Inner activation which shall be disposed by the caller</returns>
        public static ActivationBlock CreateActivationBlockInGameScope(ActivationBlock block, long gameId)
        {
            var gameManagement = block.Get<IGameManagement>();
            return CreateActivationBlockInGameScope(block, gameManagement, gameId);
        }
    }
}
