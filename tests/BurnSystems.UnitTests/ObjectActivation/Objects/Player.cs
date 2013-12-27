using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    public class Player
    {
        [Inject]
        public IDatabase Database
        {
            get;
            set;
        }

        public long PlayerId
        {
            get;
            set;
        }

        public Player(long playerId)
        {
            this.PlayerId = playerId;
        }

        public Town GetTown()
        {
            return new Town(this.Database.GetTownId(this.PlayerId));
        }
    }
}
