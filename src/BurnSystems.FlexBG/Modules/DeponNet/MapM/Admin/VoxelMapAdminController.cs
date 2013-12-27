using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapM.Admin
{
    public class VoxelMapAdminController : Controller
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

        [WebMethod]
        public IActionResult RequestInfoForColumn([PostModel] RequestInfoForColumnModel model)
        {
            var fieldInfos = this.VoxelMap.GetColumn(model.InstanceId, model.X, model.Y);
            if (fieldInfos == null)
            {
                return this.Json(new { success = false });
            }

            return this.Json(
                new
                {
                    success = true,
                    fieldInfos = fieldInfos.Changes.Select(x =>
                        new
                        {
                            changeHeight = x.ChangeHeight,
                            fieldType = x.FieldType
                        })
                });
        }

        public class RequestInfoForColumnModel
        {
            public long InstanceId
            {
                get;
                set;
            }

            public int X
            {
                get;
                set;
            }

            public int Y
            {
                get;
                set;
            }
        }
    }
}
