﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudFine
{
    [System.Serializable]
    public class SeekBehavior : GlobalRadialSteeringBehavior
    {
        public const string targetIDAttributeName = "seekTargetID";

        public override void GetSteeringBehaviorVector(out Vector3 steer, SteeringAgent mine, SurroundingsContainer surroundings)
        {
            if (!mine.HasAgentProperty(targetIDAttributeName)) mine.SetAgentProperty(targetIDAttributeName, -1);
            int chosenTargetID = mine.GetAgentProperty<int>(targetIDAttributeName);

            HashSet<Agent> allTargets = GetFilteredAgents(surroundings, this);

            if (allTargets.Count == 0)
            {
                if (chosenTargetID != -1)
                {
                    DisengagePursuit(mine, chosenTargetID);
                }
                steer = Vector3.zero;
                return;
            }

            Agent closestTarget = ClosestPursuableTarget(allTargets, mine);

            if (!closestTarget || !closestTarget.CanBeCaughtBy(mine))
            {
                if (chosenTargetID != -1)
                {
                    DisengagePursuit(mine, chosenTargetID);
                }
                steer = Vector3.zero;
                return;
            }


            if (closestTarget.agentID != chosenTargetID)
            {
                DisengagePursuit(mine, chosenTargetID);
                EngagePursuit(mine, closestTarget);
            }

            AttemptCatch(mine, closestTarget);
            Vector3 desired_velocity = (closestTarget.Position - mine.Position).normalized * mine.activeSettings.maxSpeed;
            steer = desired_velocity - mine.Velocity;
            steer = steer.normalized * Mathf.Min(steer.magnitude, mine.activeSettings.maxForce);
        }

        public static bool HasPursuitTarget(SteeringAgent mine)
        {
            if (!mine.HasAgentProperty(targetIDAttributeName)) return false;
            return mine.GetAgentProperty<int>(targetIDAttributeName) >= 0;
        }

        protected static void EngagePursuit(SteeringAgent mine, Agent target)
        {
            mine.SetAgentProperty(targetIDAttributeName, target.agentID);
        }

        protected static void DisengagePursuit(SteeringAgent mine, int targetID)
        {
            mine.SetAgentProperty(targetIDAttributeName, -1);
        }

        protected static void AttemptCatch(SteeringAgent mine, Agent target)
        {
            if (mine.Overlaps(target))
            {
                mine.CatchAgent(target);
            }
        }

        public static Agent ClosestPursuableTarget(HashSet<Agent> nearbyTargets, Agent agent)
        {
            if (nearbyTargets.Count == 0) return null;
            float closeDist = float.MaxValue;
            Agent closeTarget = null;
            foreach (Agent target in nearbyTargets)
            {
                float sqrDist = Vector3.SqrMagnitude(target.Position - agent.Position);
                if (sqrDist < closeDist && (target.CanBeCaughtBy(agent)))
                {
                    closeDist = sqrDist;
                    closeTarget = target;
                }
            }
            return closeTarget;
        }
    }
}