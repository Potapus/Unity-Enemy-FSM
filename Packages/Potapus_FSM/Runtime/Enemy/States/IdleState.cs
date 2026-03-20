using EnemyFSM.Core;
using UnityEngine;

namespace EnemyFSM.States
{
    // Stands still then wanders after a random delay.
    // Chases if player gets close, returns home if it drifted too far.
    public class IdleState : IState<EnemyContext>
    {
        private float _wanderAt;

        public void Enter(EnemyContext ctx)
        {
            if (ctx.IsAgentReady)
            {
                ctx.Agent.isStopped = true;
                ctx.Agent.ResetPath();
            }

            ScheduleWander(ctx);
        }

        public void Tick(EnemyContext ctx)
        {
            if (ctx.DistanceFromHome > ctx.Stats.HomeRadius)
            {
                ctx.StateMachine.TransitionTo(EnemyStateID.ReturnHome);
                return;
            }

            if (ctx.DistanceToTarget <= ctx.Stats.AggroRange)
            {
                ctx.StateMachine.TransitionTo(EnemyStateID.Chase);
                return;
            }

            if (Time.time >= _wanderAt)
                ctx.StateMachine.TransitionTo(EnemyStateID.Wander);
        }

        public void Exit(EnemyContext ctx)
        {
            if (ctx.IsAgentReady)
                ctx.Agent.isStopped = false;
        }

        private void ScheduleWander(EnemyContext ctx) =>
            _wanderAt = Time.time + Random.Range(ctx.Stats.MinWanderDelay, ctx.Stats.MaxWanderDelay);
    }
}
