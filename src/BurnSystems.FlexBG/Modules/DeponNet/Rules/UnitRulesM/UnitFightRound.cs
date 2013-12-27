using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.UnitRulesM
{
    public class UnitFightRound
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private ClassLogger logger = new ClassLogger(typeof(UnitFightRound));

        [Inject]
        public IUnitManagement UnitManagement
        {
            get;
            set;
        }

        [Inject]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject(ByName = "CurrentGame")]
        public long CurrentGameId
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IUnitTypeProvider UnitTypeProvider
        {
            get;
            set;
        }

        private object syncRoot = new object();

        /// <summary>
        /// Executes the fight round
        /// </summary>
        public void ExecuteFightRound()
        {
            logger.LogEntry(LogLevel.Notify, "Starting FightRound");
            var units = this.UnitManagement.GetAllUnits().ToList();
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var hasFought = false;
            Parallel.ForEach(units, (unit) =>
            {
                // Get all units in scope
                var fightingUnits = units.Where(
                    x => this.MightFight(unit, x) && x != unit)
                    .ToList();

                /*foreach (var fightUnit in fightingUnits)
                {
                    logger.LogEntry(LogLevel.Verbose, string.Format("{0} attacks {1}", unit.Id, fightUnit.Id));
                }*/

                if (fightingUnits.Count > 0)
                {
                    this.ExecuteAttack(unit, fightingUnits);
                    hasFought = true;
                }
            });

            // Removes all dead instances
            if (hasFought)
            {
                foreach (var unit in units.ToList())
                {
                    this.UnitManagement.RemoveDeadInstances(unit.Id);
                }
            }

            stopWatch.Stop();
            logger.LogEntry(LogLevel.Notify, string.Format("Ending FightRound: {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Executes the attack for the unit
        /// </summary>
        /// <param name="unit">Unit which is attacking the other units</param>
        /// <param name="fightingUnits">The units which are fought</param>
        private void ExecuteAttack(Unit unit, IList<Unit> fightingUnits)
        {
            var attackerType = this.UnitTypeProvider.Get(unit.UnitTypeId);
            if (attackerType == null)
            {
                throw new InvalidOperationException("Unknown unittype: " + unit.UnitTypeId);
            }

            // Do this for every nondead unit
            var totalCount = unit.Instances.Count;
            var totalDefenderUnits = fightingUnits.Sum(x => x.Instances.Count);

            for (var n = 0; n < totalCount; n++)
            //Parallel.For(0, totalCount, (n) =>
            {
                // Get some of all unitinstances
                var selection = MathHelper.Random.Next(0, totalDefenderUnits);

                // Find
                foreach (var defender in fightingUnits)
                {
                    if (selection >= defender.Instances.Count)
                    {
                        // Not in this defender thing, look into next one
                        selection -= defender.Instances.Count;
                    }
                    else
                    {
                        var defenderType = this.UnitTypeProvider.Get(defender.UnitTypeId);
                        var selectedInstance = defender.Instances[selection];

                        // Calculate loss
                        var loss = attackerType.AttackPoints * attackerType.AttackPoints
                            / (attackerType.AttackPoints * defenderType.DefensePoints);
                        lock (selectedInstance)
                        {
                            selectedInstance.LifePoints -= loss;

                            /*logger.LogEntry(
                                LogLevel.Verbose,
                                string.Format(
                                    "Unit {0} [{3}] reduced by {1} lifepoints, caused by {2}",
                                    defender.Id,
                                    loss,
                                    attackerType.Id,
                                    selection));*/

                            // Store result
                            // this.UnitManagement.SetUnitInstance(defender.Id, selection, selectedInstance);
                        }

                        // Cancel
                        break;
                    }
                }
            }
            //);
        }

        /// <summary>
        /// Checks, if both units might fight
        /// </summary>
        /// <param name="attacker">Attacker unit</param>
        /// <param name="defender">Defender unit</param>
        /// <returns>true, if both units fight</returns>
        public bool MightFight(Unit attacker, Unit defender)
        {
            // We do not have a strategy
            if (attacker.Strategy == null)
            {
                return false;
            }

            // Distance is too high
            var distance = (attacker.Position - defender.Position).Length;
            if (distance > attacker.Strategy.AttackRadius)
            {
                return false;
            }

            // Same player
            if (attacker.OwnerId == defender.OwnerId)
            {
                return false;
            }

            return true;
        }
    }
}
