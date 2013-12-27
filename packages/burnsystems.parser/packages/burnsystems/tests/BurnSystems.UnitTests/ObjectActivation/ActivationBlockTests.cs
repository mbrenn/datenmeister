using System;
using NUnit.Framework;
using BurnSystems.ObjectActivation;
using BurnSystems.UnitTests.ObjectActivation.Objects;

namespace BurnSystems.UnitTests.ObjectActivation
{
    [TestFixture]
    public class ActivationBlockTests
    {
        [Test]
        public void TestCreationAsSingleton()
        {
            Helper.Reset();

            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<Helper>().To<Helper>().AsSingleton();

            using (var block = new ActivationBlock("Block", activationContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(0));
        }

        [Test]
        public void TestCreationAsTransient()
        {
            Helper.Reset();

            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<Helper>().To<Helper>().AsTransient();

            using (var block = new ActivationBlock("Block", activationContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.AreNotSame(helper1, helper2);
                Assert.AreNotSame(helper2, helper3);
                Assert.AreNotSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(3));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(3));
        }

        [Test]
        public void TestCreationAsScoped()
        {
            Helper.Reset();

            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<Helper>().To<Helper>().AsScoped();

            //////////////////
            // First activation
            using (var block = new ActivationBlock("Block", activationContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(1));

            //////////////
            // Second activation
            using (var block = new ActivationBlock("Block", activationContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(2));
                Assert.That(Helper.DisposeCount, Is.EqualTo(1));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(2));
        }

        [Test]
        public void TestCreationAsTransientInParent()
        {
            Helper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest", outerContainer);
            outerContainer.Bind<Helper>().To<Helper>().AsTransient();

            using (var block = new ActivationBlock("Block", innerContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.That(helper1, Is.Not.Null);
                Assert.That(helper2, Is.Not.Null);
                Assert.That(helper3, Is.Not.Null);

                Assert.AreNotSame(helper1, helper2);
                Assert.AreNotSame(helper2, helper3);
                Assert.AreNotSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(3));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(3));
        }

        [Test]
        public void TestCreationAsSingletonInParent()
        {
            Helper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest", outerContainer);
            outerContainer.Bind<Helper>().To<Helper>().AsSingleton();

            using (var block = new ActivationBlock("Block", innerContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.That(helper1, Is.Not.Null);
                Assert.That(helper2, Is.Not.Null);
                Assert.That(helper3, Is.Not.Null);

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(0));
        }

        [Test]
        public void TestCreationAsScopedInParent()
        {
            Helper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest", outerContainer);
            outerContainer.Bind<Helper>().To<Helper>().AsScoped();

            using (var block = new ActivationBlock("Block", innerContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.That(helper1, Is.Not.Null);
                Assert.That(helper2, Is.Not.Null);
                Assert.That(helper3, Is.Not.Null);

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(1));
        }

        [Test]
        public void TestParentBlockAsScoped()
        {
            Helper.Reset();
            SecondHelper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest");
            outerContainer.Bind<Helper>().To<Helper>().AsScoped();
            innerContainer.Bind<SecondHelper>().To<SecondHelper>().AsScoped();

            using (var outerBlock = new ActivationBlock("OuterBlock", outerContainer))
            {
                using (var innerBlock = new ActivationBlock("InnerBlock", innerContainer, outerBlock))
                {
                    var helpero1 = innerBlock.Get<Helper>();
                    var helpero2 = innerBlock.Get<Helper>();
                    var helpero3 = innerBlock.Get<Helper>();
                    var helperi1 = innerBlock.Get<SecondHelper>();
                    var helperi2 = innerBlock.Get<SecondHelper>();

                    Assert.That(helpero1, Is.Not.Null);
                    Assert.That(helpero2, Is.Not.Null);
                    Assert.That(helpero3, Is.Not.Null);
                    Assert.That(helperi1, Is.Not.Null);
                    Assert.That(helperi2, Is.Not.Null);

                    Assert.AreSame(helpero1, helpero2);
                    Assert.AreSame(helpero2, helpero3);
                    Assert.AreSame(helpero1, helpero3);
                    Assert.AreSame(helperi1, helperi2);
                    Assert.AreNotSame(helperi1, helpero1);

                    Assert.That(Helper.CreationCount, Is.EqualTo(1));
                    Assert.That(Helper.DisposeCount, Is.EqualTo(0));

                    Assert.That(SecondHelper.CreationCount, Is.EqualTo(1));
                    Assert.That(SecondHelper.DisposeCount, Is.EqualTo(0));
                }
                
                Assert.That(SecondHelper.DisposeCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(1));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(1));
            Assert.That(SecondHelper.DisposeCount, Is.EqualTo(1));
        }

        [Test]
        public void TestParentBlockAsScopedIn()
        {
            Helper.Reset();
            SecondHelper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest");
            outerContainer.Bind<Helper>().To<Helper>().AsScopedIn("OuterBlock");
            innerContainer.Bind<SecondHelper>().To<SecondHelper>().AsScoped();

            using (var outerBlock = new ActivationBlock("OuterBlock", outerContainer))
            {
                using (var innerBlock = new ActivationBlock("InnerBlock", innerContainer, outerBlock))
                {
                    var helpero1 = innerBlock.Get<Helper>();
                    var helpero2 = innerBlock.Get<Helper>();
                    var helpero3 = innerBlock.Get<Helper>();
                    var helperi1 = innerBlock.Get<SecondHelper>();
                    var helperi2 = innerBlock.Get<SecondHelper>();

                    Assert.That(helpero1, Is.Not.Null);
                    Assert.That(helpero2, Is.Not.Null);
                    Assert.That(helpero3, Is.Not.Null);
                    Assert.That(helperi1, Is.Not.Null);
                    Assert.That(helperi2, Is.Not.Null);

                    Assert.AreSame(helpero1, helpero2);
                    Assert.AreSame(helpero2, helpero3);
                    Assert.AreSame(helpero1, helpero3);
                    Assert.AreSame(helperi1, helperi2);
                    Assert.AreNotSame(helperi1, helpero1);

                    Assert.That(Helper.CreationCount, Is.EqualTo(1));
                    Assert.That(Helper.DisposeCount, Is.EqualTo(0));

                    Assert.That(SecondHelper.CreationCount, Is.EqualTo(1));
                    Assert.That(SecondHelper.DisposeCount, Is.EqualTo(0));
                }

                Assert.That(SecondHelper.DisposeCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(1));
            Assert.That(SecondHelper.DisposeCount, Is.EqualTo(1));
        }

        [Test]
        public void TestParentBlockAsTransient()
        {
            Helper.Reset();
            SecondHelper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest");
            outerContainer.Bind<Helper>().To<Helper>().AsTransient();
            innerContainer.Bind<SecondHelper>().To<SecondHelper>().AsTransient();

            using (var outerBlock = new ActivationBlock("OuterBlock", outerContainer))
            {
                using (var innerBlock = new ActivationBlock("InnerBlock", innerContainer, outerBlock))
                {
                    var helpero1 = innerBlock.Get<Helper>();
                    var helpero2 = innerBlock.Get<Helper>();
                    var helpero3 = innerBlock.Get<Helper>();
                    var helperi1 = innerBlock.Get<SecondHelper>();
                    var helperi2 = innerBlock.Get<SecondHelper>();

                    Assert.That(helpero1, Is.Not.Null);
                    Assert.That(helpero2, Is.Not.Null);
                    Assert.That(helpero3, Is.Not.Null);
                    Assert.That(helperi1, Is.Not.Null);
                    Assert.That(helperi2, Is.Not.Null);

                    Assert.AreNotSame(helpero1, helpero2);
                    Assert.AreNotSame(helpero2, helpero3);
                    Assert.AreNotSame(helpero1, helpero3);
                    Assert.AreNotSame(helperi1, helperi2);
                    Assert.AreNotSame(helperi1, helpero1);

                    Assert.That(Helper.CreationCount, Is.EqualTo(3));
                    Assert.That(Helper.DisposeCount, Is.EqualTo(0));

                    Assert.That(SecondHelper.CreationCount, Is.EqualTo(2));
                    Assert.That(SecondHelper.DisposeCount, Is.EqualTo(0));
                }

                Assert.That(Helper.DisposeCount, Is.EqualTo(3));
                Assert.That(SecondHelper.DisposeCount, Is.EqualTo(2));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(3));
            Assert.That(SecondHelper.DisposeCount, Is.EqualTo(2));
        }

        [Test]
        public void TestWithConstructorInjection()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>().AsTransient();
            activationContainer.Bind<ConstructorTest>().To<ConstructorTest>();

            using (var block = new ActivationBlock("TestBlock", activationContainer))
            {
                var instanceBuilder = new InstanceBuilder(block);
                var constructorTest = instanceBuilder.Create<ConstructorTestContainer>();
                Assert.That(constructorTest, Is.Not.Null);
                Assert.That(constructorTest.Test, Is.Not.Null);
                Assert.That(constructorTest.Test.Calculator, Is.Not.Null);
                Assert.That(constructorTest.Test.IsConstructed, Is.True);
            }
        }

        [Test]
        public void TestGetWithPropertyInject()
        {
            // Initial creation
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>();
            activationContainer.Bind<CalculationContainer>().To<CalculationContainer>();

            using (var block = new ActivationBlock("TestBlock", activationContainer))
            {
                var container = block.Get<CalculationContainer>();
                Assert.That(container, Is.Not.Null);
                Assert.That(container.Calculator, Is.Not.Null);
            }
        }

        [Test]
        public void TestGetWithPropertyInjectAndConstructor()
        {
            // Initial creation
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>();
            activationContainer.Bind<ConstructorTestWithProperty>().To<ConstructorTestWithProperty>();

            using (var block = new ActivationBlock("TestBlock", activationContainer))
            {
                var container = block.Get<ConstructorTestWithProperty>();
                Assert.That(container, Is.Not.Null);
                Assert.That(container.Calculator1, Is.Not.Null);
                Assert.That(container.Calculator2, Is.Not.Null);
                Assert.That(container.Calculator3, Is.Null);

                container = block.Create<ConstructorTestWithProperty>();
                Assert.That(container, Is.Not.Null);
                Assert.That(container.Calculator1, Is.Not.Null);
                Assert.That(container.Calculator2, Is.Not.Null);
                Assert.That(container.Calculator3, Is.Null);
            }
        }

        /// <summary>
        /// Performs a test, which checks that the inner block of an ActivationBlock is also evaluated, if a single instance is queries
        /// </summary>
        [Test]
        public void TestWithInnerBlockOuterInner()
        {
            var container = new ActivationContainer("Outer Container");
            container.Bind<CalculationContainer>().To<CalculationContainer>().AsSingleton();

            using (var block = new ActivationBlock("Outer Block", container))
            {
                block.Get<Calculator>();
                var innerContainer = new ActivationContainer("Inner Container", container);
                innerContainer.Bind<ICalculator>().To<Calculator>().AsSingleton();

                using (var innerBlock = new ActivationBlock("Inner Block", innerContainer))
                {
                    var builder = new InstanceBuilder(innerBlock);
                    var containerContainer = builder.Create<CalculationContainerContainer>();

                    Assert.That(containerContainer, Is.Not.Null);
                    Assert.That(containerContainer.Container, Is.Not.Null);
                    Assert.That(containerContainer.Container.Calculator, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Performs a test, which checks that the inner block of an ActivationBlock is also evaluated, if a single instance is queries
        /// </summary>
        [Test]
        public void TestWithInnerBlockInnerOuter()
        {
            var container = new ActivationContainer("Outer Container");
            container.Bind<ICalculator>().To<Calculator>().AsSingleton();            

            using (var block = new ActivationBlock("Outer Block", container))
            {
                block.Get<Calculator>();
                var innerContainer = new ActivationContainer("Inner Container", container);
                innerContainer.Bind<CalculationContainer>().To<CalculationContainer>().AsSingleton();    

                using (var innerBlock = new ActivationBlock("Inner Block", innerContainer))
                {
                    var builder = new InstanceBuilder(innerBlock);
                    var containerContainer = builder.Create<CalculationContainerContainer>();

                    Assert.That(containerContainer, Is.Not.Null);
                    Assert.That(containerContainer.Container, Is.Not.Null);
                    Assert.That(containerContainer.Container.Calculator, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Performs a test, which checks that the inner block of an ActivationBlock is also evaluated, if a single instance is queries
        /// </summary>
        [Test]
        public void TestWithInnerBlockWithConstructor()
        {
            var container = new ActivationContainer("Outer Container");
            container.Bind<CalculationContainerWithConstructor>().To<CalculationContainerWithConstructor>().AsSingleton();

            using (var block = new ActivationBlock("Outer Block", container))
            {
                block.Get<Calculator>();
                var innerContainer = new ActivationContainer("Inner Container", container);
                innerContainer.Bind<ICalculator>().To<Calculator>().AsSingleton();

                using (var innerBlock = new ActivationBlock("Inner Block", innerContainer))
                {
                    var builder = new InstanceBuilder(innerBlock);
                    var containerContainer = builder.Create<CalculationContainerContainerWithConstructor>();

                    Assert.That(containerContainer, Is.Not.Null);
                    Assert.That(containerContainer.Container, Is.Not.Null);
                    Assert.That(containerContainer.Container.Calculator, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Performs a test, which checks that the inner block of an ActivationBlock is also evaluated, if a single instance is queries
        /// </summary>
        [Test]
        public void TestWithInnerBlockWithConstructorReverse()
        {
            var container = new ActivationContainer("Outer Container");
            container.Bind<ICalculator>().To<Calculator>().AsSingleton();

            using (var block = new ActivationBlock("Outer Block", container))
            {
                block.Get<Calculator>();
                var innerContainer = new ActivationContainer("Inner Container", container);
                innerContainer.Bind<CalculationContainerWithConstructor>().To<CalculationContainerWithConstructor>().AsSingleton();

                using (var innerBlock = new ActivationBlock("Inner Block", innerContainer))
                {
                    var builder = new InstanceBuilder(innerBlock);
                    var containerContainer = builder.Create<CalculationContainerContainerWithConstructor>();

                    Assert.That(containerContainer, Is.Not.Null);
                    Assert.That(containerContainer.Container, Is.Not.Null);
                    Assert.That(containerContainer.Container.Calculator, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Performs a test, which checks that the inner block of an ActivationBlock is also evaluated, if a single instance is queries
        /// </summary>
        [Test]
        public void TestWithInnerBlockWithConstructorButNoContainerRelationship()
        {
            var container = new ActivationContainer("Outer Container");
            container.Bind<CalculationContainerWithConstructor>().To<CalculationContainerWithConstructor>().AsSingleton();
            container.Bind<ICalculator>().ToConstant(new Calculator());

            using (var block = new ActivationBlock("Outer Block", container))
            {
                block.Get<Calculator>();
                var innerContainer = new ActivationContainer("Inner Container");

                using (var innerBlock = new ActivationBlock("Inner Block", innerContainer, block))
                {
                    var builder = new InstanceBuilder(innerBlock);
                    var containerContainer = builder.Create<CalculationContainerContainerWithConstructor>();

                    Assert.That(containerContainer, Is.Not.Null);
                    Assert.That(containerContainer.Container, Is.Not.Null);
                    Assert.That(containerContainer.Container.Calculator, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Helper class for counting creations and disposings
        /// </summary>
        public class Helper : IDisposable
        {
            /// <summary>
            /// Number of creations
            /// </summary>
            public static int CreationCount
            {
                get;
                set;
            }

            /// <summary>
            /// Number of disposal
            /// </summary>
            public static int DisposeCount
            {
                get;
                set;
            }

            /// <summary>
            /// Resets disposals and creation to 0
            /// </summary>
            public static void Reset()
            {
                CreationCount = 0;
                DisposeCount = 0;
            }

            /// <summary>
            /// Constructs and increases CreationCounter
            /// </summary>
            public Helper()
            {
                CreationCount++;
            }

            /// <summary>
            /// Disposes the instance and increases Disposal Counter
            /// </summary>
            public void Dispose()
            {
                DisposeCount++;
            }
        }

        /// <summary>
        /// Helper class for counting creations and disposings
        /// </summary>
        public class SecondHelper : IDisposable
        {
            /// <summary>
            /// Number of creations
            /// </summary>
            public static int CreationCount
            {
                get;
                set;
            }

            /// <summary>
            /// Number of disposal
            /// </summary>
            public static int DisposeCount
            {
                get;
                set;
            }

            /// <summary>
            /// Resets disposals and creation to 0
            /// </summary>
            public static void Reset()
            {
                CreationCount = 0;
                DisposeCount = 0;
            }

            /// <summary>
            /// Constructs and increases CreationCounter
            /// </summary>
            public SecondHelper()
            {
                CreationCount++;
            }

            /// <summary>
            /// Disposes the instance and increases Disposal Counter
            /// </summary>
            public void Dispose()
            {
                DisposeCount++;
            }
        }
    }
}
