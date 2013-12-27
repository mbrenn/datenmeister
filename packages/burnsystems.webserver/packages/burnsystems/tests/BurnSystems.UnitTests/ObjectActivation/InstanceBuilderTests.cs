using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.ObjectActivation;
using BurnSystems.UnitTests.ObjectActivation.Objects;

namespace BurnSystems.UnitTests.ObjectActivation
{
    [TestFixture]
    public class InstanceBuilderTests
    {
        [Test]
        public void TestInstanceBuilder()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>().AsTransient();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainer>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator, Is.Not.Null);
            Assert.That(container.Calculator, Is.InstanceOf<Calculator>());
        }

        [Test]
        public void TestInstanceBuilderByName()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>().AsTransient();
            activationContainer.BindToName("Add").To<Calculator>().AsTransient();
            activationContainer.BindToName("AddByTwo").To<CalculatorAddByTwo>().AsTransient();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainerByName>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.CalculatorByType, Is.Not.Null);
            Assert.That(container.Calculator, Is.Not.Null);
            Assert.That(container.CalculatorByTwo, Is.Not.Null);
            Assert.That(container.Calculator, Is.InstanceOf<Calculator>());
            Assert.That(container.CalculatorByType, Is.InstanceOf<Calculator>());
            Assert.That(container.CalculatorByTwo, Is.InstanceOf<CalculatorAddByTwo>());
        }

        [Test]
        public void TestInstanceEmbedded()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<CalculationContainer>().To<CalculationContainer>();
            activationContainer.Bind<ICalculator>().To<Calculator>();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainerContainer>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Container, Is.Not.Null);
            Assert.That(container.Container.Calculator, Is.Not.Null);
            Assert.That(container, Is.TypeOf<CalculationContainerContainer>());
            Assert.That(container.Container, Is.TypeOf<CalculationContainer>());
            Assert.That(container.Container.Calculator, Is.TypeOf<Calculator>());
        }

        [Test]
        public void TestInstanceOnlyGetter()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainerGetter>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator, Is.Null);
        }

        [Test]
        public void TestInstanceBuilderWithConstructor()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainerWithConstructor>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator, Is.Not.Null);
        }

        [Test]
        public void TestWebGameScenario()
        {
            var gameContainer = new ActivationContainer("Game");
            var rules = new Rules();
            rules.TownPoints = 3;

            gameContainer.Bind<Rules>().ToConstant(rules).AsSingleton();
            DatabaseDummy.Reset();
            gameContainer.Bind<IDatabase>().To(() =>
            {
                var db = new DatabaseDummy();
                db.Open();
                return db;
            }
            ).AsScoped();

            gameContainer.BindToName("CurrentPlayer").To((x) =>
            {
                var webContext = x.Get<WebContext>();
                var database = x.Get<IDatabase>();
                var playerId = database.GetPlayerId(webContext.UserId);
                var player = new Player(playerId);
                return player;
            }
            ).AsScoped();

            gameContainer.BindToName("CurrentTown").To((x) =>
            {
                var database = x.Get<IDatabase>();
                var player = x.GetByName<Player>("CurrentPlayer");
                var townId = database.GetTownId(player.PlayerId);
                var town = new Town(townId);
                return town;
            }
            ).AsTransient();

            // WebRequest 1
            {
                var webContainer = new ActivationContainer(
                    "Web",
                    gameContainer
                );
                webContainer.Bind<WebContext>().ToConstant(new WebContext() { UserId = 2 });

                using(var block = new ActivationBlock("WebBlock", webContainer)) {
                    var instanceBuilder = new InstanceBuilder(block);
                    var request = instanceBuilder.Create<WebRequest>();
                    Assert.That(request, Is.Not.Null);

                    Assert.That(DatabaseDummy.OpenCount, Is.EqualTo(1));
                    Assert.That(DatabaseDummy.DisposeCount, Is.EqualTo(0));
                    Assert.That(request.CurrentPlayer, Is.Not.Null);
                    Assert.That(request.CurrentPlayer.PlayerId, Is.EqualTo(3));
                    Assert.That(request.CurrentTown, Is.Not.Null);
                    Assert.That(request.CurrentTown.TownId, Is.EqualTo(5));

                    var request2 = instanceBuilder.Create<WebRequest>();
                    Assert.That(request2, Is.Not.Null);
                }

                Assert.That(DatabaseDummy.OpenCount, Is.EqualTo(1));
                Assert.That(DatabaseDummy.DisposeCount, Is.EqualTo(1));
            }

            // WebRequest 2
            {
                var webContainer = new ActivationContainer(
                    "Web",
                    gameContainer
                );
                webContainer.Bind<WebContext>().ToConstant(new WebContext() { UserId = 3 });

                using(var block = new ActivationBlock("WebBlock", webContainer)) {
                    var instanceBuilder = new InstanceBuilder(block);
                    var request = instanceBuilder.Create<WebRequest>();
                    Assert.That(request, Is.Not.Null);

                    Assert.That(DatabaseDummy.OpenCount, Is.EqualTo(2));
                    Assert.That(DatabaseDummy.DisposeCount, Is.EqualTo(1));
                    Assert.That(request.CurrentPlayer, Is.Not.Null);
                    Assert.That(request.CurrentPlayer.PlayerId, Is.EqualTo(4));
                    Assert.That(request.CurrentTown, Is.Not.Null);
                    Assert.That(request.CurrentTown.TownId, Is.EqualTo(6));

                    var request2 = instanceBuilder.Create<WebRequest>();
                    Assert.That(request2, Is.Not.Null);
                }

                Assert.That(DatabaseDummy.OpenCount, Is.EqualTo(2));
                Assert.That(DatabaseDummy.DisposeCount, Is.EqualTo(2));
            }
        }

        [Test]
        public void TestInjectOnInterfaces()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<IDatabase>().To<DatabaseDummy>();
            activationContainer.Bind<ICalculator>().To<CalculatorWithDatabase>();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainer>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator, Is.Not.Null);
            Assert.That(container.Calculator, Is.TypeOf<CalculatorWithDatabase>());

            var calculator = container.Calculator as CalculatorWithDatabase;
            Assert.That(calculator.Database, Is.Not.Null);
        }

        [Test]
        public void TestMandatory()
        {
            var activationContainer = new ActivationContainer("Test");
            var instanceBuilder = new InstanceBuilder(activationContainer);

            Assert.Throws<ObjectActivationException>(() =>
                {
                    instanceBuilder.Create<MandatoryCalculatorContainer>();
                });

            activationContainer.Bind<ICalculator>().To<Calculator>();

            var calculator = instanceBuilder.Create<MandatoryCalculatorContainer>();
            Assert.That(calculator.Calculator, Is.Not.Null);
        }
    }
}
