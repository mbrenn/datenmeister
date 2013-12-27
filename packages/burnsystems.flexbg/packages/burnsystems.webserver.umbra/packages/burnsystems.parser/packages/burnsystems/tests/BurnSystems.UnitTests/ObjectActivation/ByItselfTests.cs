using BurnSystems.ObjectActivation;
using BurnSystems.UnitTests.ObjectActivation.Objects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.UnitTests.ObjectActivation
{
    /// <summary>
    /// Performs the test, if the ByItself attribute is evaluated correctly
    /// </summary>
    [TestFixture]
    public class ByItselfTests
    {
        [Test]
        public void TestByInstanceBuilder()
        {
            var container = new ActivationContainer("Test");
            container.Bind<ICalculator>().To<Calculator>();

            var calc = container.Get<ICalculator>();
            Assert.That(calc, Is.Not.Null);

            var start = container.Create<Start>();
            Assert.That(start, Is.Not.Null);
            Assert.That(start.Intermediate, Is.Not.Null);
            Assert.That(start.Intermediate.End, Is.Not.Null);
            Assert.That(start.Intermediate.End.Calculator, Is.Not.Null);
        }

        [Test]
        public void TestByActivationContainer()
        {
            var container = new ActivationContainer("Test");
            container.Bind<ICalculator>().To<Calculator>();
            container.Bind<Start>().To<Start>();

            var calc = container.Get<ICalculator>();
            Assert.That(calc, Is.Not.Null);

            var start = container.Get<Start>();
            Assert.That(start, Is.Not.Null);
            Assert.That(start.Intermediate, Is.Not.Null);
            Assert.That(start.Intermediate.End, Is.Not.Null);
            Assert.That(start.Intermediate.End.Calculator, Is.Not.Null);
        }

        public class Start
        {
            [Inject(ByItself = true)]
            public Intermediate Intermediate
            {
                get;
                set;
            }
        }
         
        public class Intermediate
        {
            [Inject(ByItself = true)]
            public End End
            {
                get;
                set;
            }
        }

        public class End
        {
            [Inject]
            public ICalculator Calculator
            {
                get;
                set;
            }
        }
    }
}
