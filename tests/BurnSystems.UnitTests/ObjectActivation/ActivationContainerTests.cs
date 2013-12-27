using System;
using System.Linq;
using BurnSystems.ObjectActivation;
using BurnSystems.UnitTests.ObjectActivation.Objects;
using NUnit.Framework;

namespace BurnSystems.UnitTests.ObjectActivation
{
	[TestFixture]
	public class ActivationContainerTests
	{
		[Test]
		public void TestTypeCreation()
		{
			var activationContainer = new ActivationContainer("Test");
			activationContainer.Bind<ICalculator>().To<Calculator>().AsTransient();
			
			var calculator = activationContainer.Get<ICalculator>();
			Assert.That(calculator, Is.TypeOf(typeof(Calculator)));

			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(5));
		}
		
		[Test]
		public void TestConstant()
		{
			var activationContainer = new ActivationContainer("Test");
			var myCalculator = new Calculator();
			myCalculator.InternalAddOffset = 5;
			activationContainer.Bind<ICalculator>().ToConstant(myCalculator).AsTransient();
			
			var calculator = activationContainer.Get<ICalculator>();			
			Assert.That(calculator, Is.TypeOf(typeof(Calculator)));

			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(10));
		}
		
		[Test]
		public void TestFactory()
		{
			var activationContainer = new ActivationContainer("Test");
			activationContainer.Bind<ICalculator>().To(
				() => new Calculator()
				{
					InternalAddOffset = 7
				}).AsTransient();
			
			var calculator = activationContainer.Get<ICalculator>();			
			Assert.That(calculator, Is.TypeOf(typeof(Calculator)));

			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(12));
		}

        [Test]
        public void TestGetByName()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.BindToName("Calc").To(() => new Calculator()
                {
                    InternalAddOffset = 7
                }).AsTransient();

            var calculator = activationContainer.GetByName<ICalculator>("Calc");
            Assert.That(calculator, Is.TypeOf(typeof(Calculator)));
            var result = calculator.Add(2, 3);
            Assert.That(result, Is.EqualTo(12));

            var calculator2 = activationContainer.GetByName<ICalculator>("Calc2");
            Assert.That(calculator2, Is.Null);
        }
		
		
		[Test]
		public void TestTypeCreationAsSingleton()
		{
			// Initial creation
			var activationContainer = new ActivationContainer("Test");
			activationContainer.Bind<ICalculator>().To<Calculator>().AsSingleton();
			
			var calculator = activationContainer.Get<ICalculator>();
			Assert.That(calculator, Is.TypeOf(typeof(Calculator)));

			// First get
			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(5));		
			
			// Change singleton
			((Calculator) calculator).InternalAddOffset = 10;
			
			result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(15));		
			
			// Now, retrieve the variable again, we'd like to have
			// the same instance! 
			calculator = activationContainer.Get<ICalculator>();
			result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(15));					
		}

        [Test]
        public void TestTypeCreationAsScopedThrowingException()
        {
            // Initial creation
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>().AsScoped();

            Assert.Throws<InvalidOperationException>(() => activationContainer.Get<ICalculator>());
        }

        [Test]
        public void TestBindToLambda()
        {
            var outerContainer = new ActivationContainer("Inner");
            int x = 0;

            outerContainer.Bind<ICalculator>().To(() =>
                {
                    x++;
                    return new Calculator();
                }).AsTransient(); ;

            var calculator = outerContainer.Get<ICalculator>();
            Assert.That(calculator, Is.Not.Null);

            var result = calculator.Add(2, 3);
            Assert.That(result, Is.EqualTo(5));

            Assert.That(x, Is.EqualTo(1));

            var calculator2 = outerContainer.Get<ICalculator>();
            Assert.That(calculator2, Is.Not.Null);
            Assert.That(x, Is.EqualTo(2));
        }

        [Test]
        public void TestBindToLambdaWithContainer()
        {
            var outerContainer = new ActivationContainer("Inner");
            int x = 0;

            outerContainer.Bind<ICalculator>().To((a) =>
            {
                Assert.That(a, Is.AssignableTo<IActivates>());
                x++;
                return new Calculator();
            }
            ).AsTransient();
            ;

            var calculator = outerContainer.Get<ICalculator>();
            Assert.That(calculator, Is.Not.Null);

            var result = calculator.Add(2, 3);
            Assert.That(result, Is.EqualTo(5));

            Assert.That(x, Is.EqualTo(1));

            var calculator2 = outerContainer.Get<ICalculator>();
            Assert.That(calculator2, Is.Not.Null);
            Assert.That(x, Is.EqualTo(2));
        }

		[Test]
		public void TestTransientRetrievalFromParent()
		{
			var outerContainer = new ActivationContainer("Outer");
			var innerContainer = new ActivationContainer("Inner", outerContainer);
			
			outerContainer.Bind<ICalculator>().To<Calculator>();

			var calculator = innerContainer.Get<ICalculator>();
			Assert.That(calculator, Is.Not.Null);
			
			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(5));
		}

		[Test]
		public void TestTransientRetrievalFromParentOverride()
		{
			var outerContainer = new ActivationContainer("Outer");
			var innerContainer = new ActivationContainer("Inner", outerContainer);
			
			outerContainer.Bind<ICalculator>().To<Calculator>();
			innerContainer.Bind<ICalculator>().To(
				() => new Calculator()
				{
					InternalAddOffset = 5
				});

			// Inner Calculator is Adder with offset = 5
			var innerCalculator = innerContainer.Get<ICalculator>();
			Assert.That(innerCalculator, Is.Not.Null);
			
			var result = innerCalculator.Add(2,3);
			Assert.That(result, Is.EqualTo(10));
			
			// Outer Calculator is Adder with offset = 0
			var outerCalculator = outerContainer.Get<ICalculator>();
			Assert.That(outerCalculator, Is.Not.Null);
			
			var result2 = outerCalculator.Add(2,3);
			Assert.That(result2, Is.EqualTo(5));
		}
		
		[Test]
		public void TestSingletonRetrievalFromParentOverride()
		{
			var outerContainer = new ActivationContainer("Outer");
			var innerContainer = new ActivationContainer("Inner", outerContainer);
			
			outerContainer.Bind<ICalculator>().To<Calculator>().AsSingleton();
			innerContainer.Bind<ICalculator>().To(
				() => new Calculator()
				{
					InternalAddOffset = 5
				})
				.AsSingleton();
			
			var innerCalculator = innerContainer.Get<ICalculator>();
			var outerCalculator = outerContainer.Get<ICalculator>();

			var innerResult = innerCalculator.Add(5,3);
			var outerResult = outerCalculator.Add(5,3);
			
			Assert.That(innerResult, Is.EqualTo(13));
			Assert.That(outerResult, Is.EqualTo(8));
			
			// Change behaviour of instance
			((Calculator)innerCalculator).InternalAddOffset = 10;			
			
			innerCalculator = innerContainer.Get<ICalculator>();
			outerCalculator = outerContainer.Get<ICalculator>();

			innerResult = innerCalculator.Add(5,3);
			outerResult = outerCalculator.Add(5,3);
			
			Assert.That(innerResult, Is.EqualTo(18));
			Assert.That(outerResult, Is.EqualTo(8));

		}

        [Test]
        public void TestGetAll()
        {
            var container = new ActivationContainer("Test");
            container.Bind<ICalculator>().To<Calculator>().AsSingleton();
            container.Bind<ICalculator>().To<CalculatorAddByTwo>().AsSingleton();

            var calculators = container.GetAll<ICalculator>().ToList();
            Assert.That(calculators.Count, Is.EqualTo(2));

            var normalCalculator = calculators.Where(x => x is Calculator).ToList();
            var otherCalculator = calculators.Where(x => x is CalculatorAddByTwo).ToList();

            Assert.That(normalCalculator, Is.Not.Null);
            Assert.That(otherCalculator, Is.Not.Null);
            Assert.That(normalCalculator.Count, Is.EqualTo(1));
            Assert.That(otherCalculator.Count, Is.EqualTo(1));                        
        }
        
        [Test]
        public void TestWithConstructorInjection()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>().AsTransient();
            activationContainer.Bind<ConstructorTest>().To<ConstructorTest>();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var constructorTest = instanceBuilder.Create<ConstructorTestContainer>();
            Assert.That(constructorTest, Is.Not.Null);
            Assert.That(constructorTest.Test, Is.Not.Null);
            Assert.That(constructorTest.Test.Calculator, Is.Not.Null);
            Assert.That(constructorTest.Test.IsConstructed, Is.True);
        }

        [Test]
        public void TestGetWithPropertyInject()
        {
            // Initial creation
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>();
            activationContainer.Bind<CalculationContainer>().To<CalculationContainer>();

            var container = activationContainer.Get<CalculationContainer>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator, Is.Not.Null);
        }

        [Test]
        public void TestGetWithPropertyInjectAndConstructor()
        {
            // Initial creation
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>();
            activationContainer.Bind<ConstructorTestWithProperty>().To<ConstructorTestWithProperty>();

            var container = activationContainer.Get<ConstructorTestWithProperty>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator1, Is.Not.Null);
            Assert.That(container.Calculator2, Is.Not.Null);
            Assert.That(container.Calculator3, Is.Null);

            container = activationContainer.Create<ConstructorTestWithProperty>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator1, Is.Not.Null);
            Assert.That(container.Calculator2, Is.Not.Null);
            Assert.That(container.Calculator3, Is.Null);
        }
    }
}
