using BurnSystems.FlexBG.Modules.DeponNet.UnitM;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using BurnSystems.FlexBG.Modules.WayPointCalculationM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.UnitRulesM
{
    internal class UnitMoveJobRule
    {
        /// <summary>
        /// Gets or sets the unit job rules
        /// </summary>
        public UnitJobRules Rules
        {
            get;
            set;
        }

        public UnitMoveJobRule(UnitJobRules rules)
        {
            this.Rules = rules;
        }

        /// <summary>
        /// Expands the move job in a way that a move job will be generated for each field
        /// </summary>
        /// <param name="unit">Unit who needs new jobs</param>
        /// <param name="moveJob">Movejob to be generated</param>
        public void ExpandMoveJob(Unit unit, MoveJob moveJob)
        {
            var currentJobPosition = unit.IndexCurrentJob;
            var wayPoints = this.Rules.WayPointCalculation.CalculateWaypoints(
                    unit.Position,
                    moveJob.TargetPosition,
                    null)
                .ToList();

            // First waypoint and last waypoint are not required, because they give the start and endposition
            foreach (var point in wayPoints.Skip(1).Take(wayPoints.Count - 2))
            {
                var newMoveJob = new MoveJob()
                {
                    IsUserDefined = false,
                    TargetPosition = point,
                    Velocity = moveJob.Velocity
                };

                this.Rules.UnitManagement.InsertJob(unit.Id, newMoveJob, currentJobPosition);

                currentJobPosition++;
            }
        }

        /// <summary>
        /// Executes the move job for the given unit
        /// </summary>
        /// <param name="unit">Unit, whose job shall be executed</param>
        /// <param name="moveJob">The movejob</param>
        public bool ExecuteMoveJob(Unit unit, MoveJob moveJob)
        {
            var tick = 1;
            var finished = false;
            var distance = moveJob.Velocity * tick;

            var currentPosition = unit.Position;
            var targetPosition = moveJob.TargetPosition;

            var wayTo = targetPosition - currentPosition;
            if (wayTo.Length <= distance)
            {
                // We have reached target position, finish!
                distance = wayTo.Length;
                finished = true;
            }

            wayTo.Normalize();
            wayTo *= distance;

            this.Rules.UnitManagement.UpdatePosition(unit.Id, currentPosition + wayTo);

            return finished;
        }
    }
}
