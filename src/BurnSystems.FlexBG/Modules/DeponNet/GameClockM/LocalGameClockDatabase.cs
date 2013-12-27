using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameClockM
{
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class LocalGameClockDatabase : IFlexBgRuntimeModule
    {
        public GameClocksData GameClockStore
        {
            get;
            set;
        }

        public void Start()
        {
            this.GameClockStore = SerializedFile.LoadFromFile<GameClocksData>("gameclocks.db", () => new GameClocksData());
        }

        public void Shutdown()
        {
            SerializedFile.StoreToFile("gameclocks.db", this.GameClockStore);
        }
    }
}
