using BurnSystems.FlexBG.Modules.DeponNet.BuildingM;
using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.MapFieldOwnerM;
using BurnSystems.FlexBG.Modules.DeponNet.MapM;
using BurnSystems.FlexBG.Modules.DeponNet.MapM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet
{
    /// <summary>
    /// Defines the game configuration
    /// </summary>
    public static class GameConfig
    {
        /// <summary>
        /// Defines the buildings configurations
        /// </summary>
        public static class Buildings
        {
            public static BuildingType Temple;

            public static BuildingType Townhall;

            public static BuildingType Coppermine;

            public static BuildingType ForestersPlace;

            public static BuildingType SawingPlace;

            public static BuildingType SmeltingPlace;

            public static BuildingType LivingHouse;
        }

        public static class Fields
        {
            /// <summary>
            /// Stores the air
            /// </summary>
            public static FieldType Air;

            /// <summary>
            /// Defines the grass field type as 1
            /// </summary>
            public static FieldType Grass;

            /// <summary>
            /// Defines the grass field type as 2
            /// </summary>
            public static FieldType DarkGrass;
        }

        public static class Resources
        {
            public static ResourceType Wood;

            public static ResourceType CopperOre;

            public static ResourceType Stone;

            public static ResourceType Girder;

            public static ResourceType Copper;

            public static ResourceType Oil;
        }

        public static class Units
        {
            public static UnitType Settler;

            public static UnitType Constructor;
        }

        public static class Rules
        {
            public static FieldOwnershipByBuildingConfig EmpireRules;

            public static int EmpireRuleDataKey = 0x1001;
        }

        /// <summary>
        /// Initializes the game configuration
        /// </summary>
        public static void Init(IActivates container)
        {
            InitBuildingTypes(container);
            InitFieldTypes(container);
            InitResourceTypes(container);
            InitUnitTypes(container);
            InitRules(container);

            var playerRules = container.Get<PlayerRulesConfig>();
            Ensure.That(playerRules != null);
            playerRules.PlayerStartResources[Resources.Wood] = 1000;
            playerRules.PlayerStartResources[Resources.CopperOre] = 1000;
            playerRules.PlayerStartResources[Resources.Stone] = 1000;
            playerRules.PlayerStartResources[Resources.Girder] = 1000;
            playerRules.PlayerStartResources[Resources.Wood] = 1000;
        }

        private static void InitResourceTypes(IActivates container)
        {
            var resourceTypeProvider = container.Get<IResourceTypeProvider>();
            Ensure.That(resourceTypeProvider != null, "No IResourceTypeProvider");

            Resources.Wood = new ResourceType();
            Resources.Wood.Id = 1;
            Resources.Wood.Token = "wood";
            resourceTypeProvider.Add(Resources.Wood);

            Resources.CopperOre = new ResourceType();
            Resources.CopperOre.Id = 2;
            Resources.CopperOre.Token = "copperore";
            resourceTypeProvider.Add(Resources.CopperOre);

            Resources.Stone = new ResourceType();
            Resources.Stone.Id = 3;
            Resources.Stone.Token = "stone";
            resourceTypeProvider.Add(Resources.Stone);

            Resources.Girder = new ResourceType();
            Resources.Girder.Id = 4;
            Resources.Girder.Token = "girder";
            resourceTypeProvider.Add(Resources.Girder);

            Resources.Copper = new ResourceType();
            Resources.Copper.Id = 5;
            Resources.Copper.Token = "copper";
            resourceTypeProvider.Add(Resources.Copper);

            Resources.Oil = new ResourceType();
            Resources.Oil.Id = 6;
            Resources.Oil.Token = "oil";
            resourceTypeProvider.Add(Resources.Oil);
        }

        private static void InitBuildingTypes(IActivates container)
        {
            var buildingTypeProvider = container.Get<IBuildingTypeProvider>();
            Ensure.That(buildingTypeProvider != null, "No IBuildingTypeProvider");

            var id = 1;
            Buildings.Temple = new BuildingType()
            {
                Id = id++,
                Token = "temple",
                SizeX = 2,
                SizeY = 2
            };
            buildingTypeProvider.Add(Buildings.Temple);

            Buildings.Townhall = new BuildingType()
            {
                Id = id++,
                Token = "townhall",
                SizeX = 2,
                SizeY = 2
            };
            buildingTypeProvider.Add(Buildings.Townhall);

            Buildings.Coppermine = new BuildingType()
            {
                Id = id++,
                Token = "coppermine",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.Coppermine);

            Buildings.ForestersPlace = new BuildingType()
            {
                Id = id++,
                Token = "forestersplace",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.ForestersPlace);

            Buildings.SawingPlace = new BuildingType()
            {
                Id = id++,
                Token = "sawingplace",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.SawingPlace);

            Buildings.SmeltingPlace = new BuildingType()
            {
                Id = id++,
                Token = "smeltingplace",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.SmeltingPlace);

            Buildings.LivingHouse = new BuildingType()
            {
                Id = id++,
                Token = "livinghouse",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.LivingHouse);
        }

        /// <summary>
        /// Initializes the field types
        /// </summary>
        /// <param name="container">Container to be used</param>
        private static void InitFieldTypes(IActivates container)
        {
            var fieldTypeProvider = container.Get<IFieldTypeProvider>();
            
            /// <summary>
            /// Stores the air
            /// </summary>
            Fields.Air = new FieldType()
            {
                Id = 0,
                Token = "air"
            };
            fieldTypeProvider.Add(Fields.Air);

            /// <summary>
            /// Defines the grass field type as 1
            /// </summary>
            Fields.Grass = new FieldType()
            {
                Id = 1,
                Token = "grass"
            };
            fieldTypeProvider.Add(Fields.Grass);

            /// <summary>
            /// Defines the grass field type as 2
            /// </summary>
            Fields.DarkGrass = new FieldType()
            {
                Id = 2,
                Token = "darkgrass"
            };
            fieldTypeProvider.Add(Fields.DarkGrass);
        }

        /// <summary>
        /// Initializes the unit types
        /// </summary>
        /// <param name="activates">Activation Container</param>
        private static void InitUnitTypes(IActivates activates)
        {
            var unitTypeProvider = activates.Get<IUnitTypeProvider>();
            Ensure.That(unitTypeProvider != null);

            Units.Settler = new UnitType()
            {
                Id = 1,
                Token = "settler", 
                Velocity = 1, 
                LifePoints = 100,
                AttackPoints = 1, 
                DefensePoints = 5
            };

            Units.Constructor = new UnitType()
            {
                Id = 2,
                Token = "constructor", 
                Velocity = 1, 
                LifePoints = 50,
                AttackPoints = 1, 
                DefensePoints = 5
            };

            unitTypeProvider.Add(Units.Settler);
            unitTypeProvider.Add(Units.Constructor);
        }

        private static void InitRules(IActivates container)
        {
            Rules.EmpireRules = new FieldOwnershipByBuildingConfig();
            Rules.EmpireRules.DataKey = Rules.EmpireRuleDataKey;
            Rules.EmpireRules.MaxRadius = 5;

            Rules.EmpireRules.AddBuilding(Buildings.Temple.Id, 10);
            Rules.EmpireRules.AddBuilding(Buildings.Townhall.Id, 5);
            Rules.EmpireRules.AddBuilding(Buildings.Coppermine.Id, 2);
            Rules.EmpireRules.AddBuilding(Buildings.ForestersPlace.Id, 2);
            Rules.EmpireRules.AddBuilding(Buildings.LivingHouse.Id, 3);
            Rules.EmpireRules.AddBuilding(Buildings.SawingPlace.Id, 2);
            Rules.EmpireRules.AddBuilding(Buildings.SmeltingPlace.Id, 2);
        }
    }
}
