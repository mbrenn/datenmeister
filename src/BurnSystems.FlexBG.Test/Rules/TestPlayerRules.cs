using BurnSystems.FlexBG.Modules.DeponNet;
using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.ObjectActivation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
namespace BurnSystems.FlexBG.Test.Rules
{
    [TestFixture]
    public class TestPlayerRules
    {
        [Test]
        public void TestPlayerCreation()
        {
            using (var rules = new RuleHelper())
            {
                var userId = rules.CreateUser();
                rules.CreatePlayer(userId);
                var playerManagement = rules.Container.Get<IPlayerManagement>();

                var players = playerManagement.GetAllPlayers();
                Assert.That(players.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public void TestTownCreation()
        {
            using (var rules = new RuleHelper())
            {
                var userId = rules.CreateUser();
                var playerId = rules.CreatePlayer(userId);
                var townManagement = rules.Container.Get<ITownManagement>();

                var towns = townManagement.GetTownsOfPlayer(playerId);
                Assert.That(towns.Count(), Is.EqualTo(1));

                var town = towns.Single();
                Assert.That(town.OwnerId, Is.EqualTo(playerId));
                Assert.That(town.IsCapital, Is.True);
            }
        }

        [Test]
        public void TestBuildingCreation()
        {
            using (var rules = new RuleHelper())
            {
                var userId = rules.CreateUser();
                var playerId = rules.CreatePlayer(userId);
                var townManagement = rules.Container.Get<ITownManagement>();
                var buildingManagement = rules.Container.Get<IBuildingManagement>();

                var towns = townManagement.GetTownsOfPlayer(playerId);
                var town = towns.First();

                var buildings = buildingManagement.GetBuildingsOfPlayer(playerId);
                Assert.That(buildings.Count(), Is.EqualTo(1));

                var buildingsInTown = buildingManagement.GetBuildingsOfTown(town.Id);
                Assert.That(buildingsInTown.Count(), Is.EqualTo(1));

                Assert.That(buildings.Single().Id == buildingsInTown.Single().Id);

                var building = buildings.Single();
                Assert.That(building.BuildingTypeId, Is.EqualTo(GameConfig.Buildings.Temple.Id));
            }
        }
    }
}
*/