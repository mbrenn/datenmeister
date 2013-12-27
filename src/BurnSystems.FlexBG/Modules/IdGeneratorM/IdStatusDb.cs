using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.IdGeneratorM
{
    public class IdStatusDb : IFlexBgRuntimeModule
    {
        public IdStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Called, when started
        /// </summary>
        public void Start()
        {
            this.Status = SerializedFile.LoadFromFile<IdStatus>("ids.db", () => new IdStatus());
        }

        /// <summary>
        /// Called when shutdowned
        /// </summary>
        public void Shutdown()
        {
            SerializedFile.StoreToFile("ids.db", this.Status);
        }
    }
}
