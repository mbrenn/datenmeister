using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.UnitTests.ObjectActivation
{
    [TestFixture]
    public class BindAlsoToTests
    {
        [Test]
        public void TestBindByType()
        {
            var container = new ActivationContainer("TEST");
            container.Bind<ITestClass>().To<TestClass>();

            Ensure.IsNotNull(container.Get<IAlsoBound>());
            Ensure.IsNotNull(container.Get<ITestClass>());

        }

        [Test]
        public void TestBindByName()
        {
            var container = new ActivationContainer("TEST");
            container.BindToName("Name").To<TestClass>();

            Ensure.IsNotNull(container.GetByName<ITestClass>("Name"));
            Ensure.IsNotNull(container.Get<IAlsoBound>());
        }

        [Test]
        public void TestBindByTypeToConstant()
        {
            var instance = new TestClass();
            var container = new ActivationContainer("TEST");
            container.Bind<ITestClass>().ToConstant(instance);

            Ensure.IsNotNull(container.Get<IAlsoBound>());
            Ensure.IsNotNull(container.Get<ITestClass>());
        }

        [Test]
        public void TestBindWithoutAutobinding()
        {
            var container = new ActivationContainer("TEST");
            container.Bind<ITestClass>().WithoutAutoBinding().To<TestClass>();

            Ensure.IsNull(container.Get<IAlsoBound>());
            Ensure.IsNotNull(container.Get<ITestClass>());
        }

        public interface IAlsoBound
        {
        }

        public interface ITestClass
        {
        }

        [BindAlsoTo(typeof(IAlsoBound))]
        public class TestClass : IAlsoBound, ITestClass
        {
        }


    }

}
