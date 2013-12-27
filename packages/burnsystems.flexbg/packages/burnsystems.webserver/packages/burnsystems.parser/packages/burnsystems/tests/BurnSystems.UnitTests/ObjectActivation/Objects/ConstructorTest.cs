using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    public class ConstructorTest
    {
        public bool IsConstructed
        {
            get;
            set;
        }

        public ICalculator Calculator
        {
            get;
            set;
        }

        public ConstructorTest()
        {
        }

        [Inject]
        public ConstructorTest(ICalculator calculator)
        {
            this.IsConstructed = true;
            this.Calculator = calculator;
        }
    }

    public class ConstructorTestWithProperty
    {
        public bool IsConstructed
        {
            get;
            set;
        }

        public ICalculator Calculator1
        {
            get;
            set;
        }

        [Inject]
        public ICalculator Calculator2
        {
            get;
            set;
        }

        public ICalculator Calculator3
        {
            get;
            set;
        }

        public ConstructorTestWithProperty()
        {
        }

        [Inject]
        public ConstructorTestWithProperty(ICalculator calculator)
        {
            this.IsConstructed = true;
            this.Calculator1 = calculator;
        }
    }


    public class ConstructorTestContainer
    {
        [Inject]
        public ConstructorTest Test
        {
            get;
            set;
        }
    }
}
