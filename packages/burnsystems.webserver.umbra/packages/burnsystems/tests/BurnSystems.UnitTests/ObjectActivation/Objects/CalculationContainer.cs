using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;
using NUnit.Framework;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    public class CalculationContainer
    {
        [Inject]
        public ICalculator Calculator
        {
            get;
            set;
        }
    }

    public class CalculationContainerGetter
    {
        [Inject]
        public ICalculator Calculator
        {
            get;
            private set;
        }
    }

    public class CalculationContainerByName
    {
        [Inject]
        public ICalculator CalculatorByType
        {
            get;
            set;
        }

        [Inject("Add")]
        public ICalculator Calculator
        {
            get;
            set;
        }

        [Inject("AddByTwo")]
        public ICalculator CalculatorByTwo
        {
            get;
            set;
        }
    }

    public class CalculationContainerContainer
    {
        [Inject]
        public CalculationContainer Container
        {
            get;
            set;
        }
    }
    
    public class CalculationContainerWithConstructor
    {
        public ICalculator Calculator
        {
            get;
            set;
        }

        [Inject]
        public CalculationContainerWithConstructor(ICalculator calculator)
        {
            Assert.That(calculator != null);
            this.Calculator = calculator;
        }
    }

    public class CalculationContainerContainerWithConstructor
    {
        [Inject]
        public CalculationContainerWithConstructor Container
        {
            get;
            set;
        }
    }
}
