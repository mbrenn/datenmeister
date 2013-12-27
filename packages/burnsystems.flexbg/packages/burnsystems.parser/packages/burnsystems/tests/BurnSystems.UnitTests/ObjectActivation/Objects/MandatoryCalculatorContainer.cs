using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    /// <summary>
    /// Defines a container that requires a calculator by mandatory information
    /// </summary>
    public class MandatoryCalculatorContainer
    {
        [Inject(IsMandatory = true)]
        public ICalculator Calculator
        {
            get;
            set;
        }

    }
}
