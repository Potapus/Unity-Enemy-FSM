using EnemyFSM.Core;

namespace EnemyFSM.States
{
    // Walks back to the spawn point. Resumes chase if the player gets close again.
    public class ReturnHomeState : IState<EnemyContext>
    {
        public void Enter(EnemyContext ctx)
        {
            if (!ctx.IsAgentReady) return;

            ctx.Agent.isStopped        = false;
            ctx.Agent.stoppingDistance = 0f;
            ctx.Agent.SetDestination(ctx.HomePosition);
        }

        public void Tick(EnemyContext ctx)
        {
            if (ctx.DistanceToTarget <= ctx.Stats.AggroRange)
            {
                ctx.StateMachine.TransitionTo(EnemyStateID.Chase);
                return;
            }

            if (!ctx.IsAgentReady) return;

            if (!ctx.Agent.pathPending && ctx.Agent.remainingDistance <= ctx.Stats.DestinationReachedDistance)
                ctx.StateMachine.TransitionTo(EnemyStateID.Idle);
        }

        public void Exit(EnemyContext ctx) { }
    }
}
