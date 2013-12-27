using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Controllers;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.DataProvider
{
    public class MapInfoController : Controller
    {
        [Inject]
        public IVoxelMap VoxelMap
        {
            get;
            set;
        }

        [Inject(ByName = VoxelMapController.CurrentMapInstanceName, IsMandatory = true)]
        public VoxelMapInfo CurrentMapInstance
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult GetMapInfo()
        {
            if (this.CurrentMapInstance == null)
            {
                throw new MVCProcessException("nomapinfo", "No mapinfo retrieved");
            }
            else
            {
                return this.Json(new
                {
                    success = true,
                    sizeX = this.CurrentMapInstance.SizeX,
                    sizeY = this.CurrentMapInstance.SizeY
                });
            }
        }

        /// <summary>
        /// Gets the surface for the map
        /// </summary>
        /// <param name="i">Instance of the map</param>
        /// <param name="x1">Left-X-Coordinate of the map</param>
        /// <param name="x2">Right-X-Coordinate of the map</param>
        /// <param name="y1">Top-Y-Coordinate of the map</param>
        /// <param name="y2">Bottom-Y-Coordinate of the map</param>
        /// <returns></returns>
        [WebMethod]
        public IActionResult GetSurface(int x1, int x2, int y1, int y2)
        {
            if (x2 < x1 || y2 < y1)
            {
                throw new ArgumentException("x2 < x1 || y2 < y1");
            }

            var fields = (x2 - x1) * (y2 - y1);
            if (fields > 10000)
            {
                throw new ArgumentException("(x2 - x1) * (y2 - y1) > 10000");
            }

            var surface = this.VoxelMap.GetSurfaceInfo(this.CurrentMapInstance.InstanceId, x1, y1, x2, y2);

            var result = new List<object>();

            var tx = 0;
            for (var x = x1; x <= x2; x++)
            {
                var ty = 0;
                for (var y = y1; y <= y2; y++)
                {
                    var ownerId = this.VoxelMap.GetDataAsInt64(
                        this.CurrentMapInstance.InstanceId,
                        x,
                        y,
                        GameConfig.Rules.EmpireRuleDataKey);

                    result.Add(
                        new
                        {
                            x = x,
                            y = y,
                            h = surface[tx][ty].ChangeHeight,
                            t = surface[tx][ty].FieldType,
                            o = ownerId
                        });

                    ty++;
                }


                tx++;
            }

            return this.Json(
                new
                {
                    success = true,
                    fields = result
                });
        }
    }
}