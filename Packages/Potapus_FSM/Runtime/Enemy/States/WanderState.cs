using EnemyFSM.Core;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyFSM.States
{
    // Picks a random NavMesh point near home and walks there.
    // Goes back to Idle on arrival, or Chase if the player gets close.
    public class WanderState : IState<EnemyContext>
    {
        public void Enter(EnemyContext ctx)
        {
            if (!ctx.IsAgentReady) return;

            if (TryGetRandomNavMeshPoint(ctx, out Vector3 point))
                ctx.Agent.SetDestination(point);
            else
                ctx.StateMachine.TransitionTo(EnemyStateID.Idle);
        }

        public void Tick(EnemyContext ctx)
        {
            if (ctx.DistanceToTarget <= ctx.Stats.AggroRange)
            {
                ctx.StateMachine.TransitionTo(EnemyStateID.Chase);
                return;
            }

            if (ctx.DistanceFromHome > ctx.Stats.HomeRadius)
            {
                ctx.StateMachine.TransitionTo(EnemyStateID.ReturnHome);
                return;
            }

            if (!ctx.IsAgentReady) return;

            if (!ctx.Agent.pathPending && ctx.Agent.remainingDistance <= ctx.Stats.DestinationReachedDistance)
                ctx.StateMachine.TransitionTo(EnemyStateID.Idle);
        }

        public void Exit(EnemyContext ctx) { }

        // Tries up to 10 times to find a valid NavMesh point near home.
        private static bool TryGetRandomNavMeshPoint(EnemyContext ctx, out Vector3 result)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 rand      = Random.insideUnitCircle * ctx.Stats.WanderRadius;
                Vector3 candidate = ctx.HomePosition + new Vector3(rand.x, 0f, rand.y);

                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, ctx.Stats.NavMeshSampleRadius, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }

            result = ctx.HomePosition;
            return false;
        }
    }
}
