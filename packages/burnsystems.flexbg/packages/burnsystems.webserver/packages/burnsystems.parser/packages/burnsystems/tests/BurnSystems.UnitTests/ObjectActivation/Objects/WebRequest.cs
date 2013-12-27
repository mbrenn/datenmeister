using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    public class WebRequest
    {
        [Inject("CurrentPlayer")]
        public Player CurrentPlayer
        {
            get;
            set;
        }

        [Inject("CurrentTown")]
        public Town CurrentTown
        {
            get;
            set;
        }
    }
}
