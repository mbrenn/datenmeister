using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.MapM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.DataProvider
{
    /// <summary>
    /// Some information about game information
    /// </summary>
    public class GameInfoController : Controller
    {
        [Inject]
        public IResourceTypeProvider ResourceTypeProvider
        {
            get;
            set;
        }

        [Inject]
        public IFieldTypeProvider FieldTypeProvider
        {
            get;
            set;
        }

        [Inject]
        public IGameManagement GameManagement
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
        /// Gets information about the current game. 
        /// Includes resource types
        /// </summary>
        [WebMethod]
        public IActionResult GetCurrentGame()
        {
            var fieldTypes = this.FieldTypeProvider.GetAll().Select(x =>
                new
                {
                    id = x.Id,
                    token = x.Token
                });

            var resourceTypes = this.ResourceTypeProvider.GetAll().Select(x =>
                new
                {
                    id = x.Id,
                    token = x.Token
                });

            var result = new
            {
                fieldTypes = fieldTypes,
                resourceTypes = resourceTypes,
                game = this.CurrentGame.AsJson(),
                success = true
            };

            return this.Json(result);
        }
    }
}
