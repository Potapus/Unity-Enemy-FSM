using EnemyFSM.Core;
using UnityEngine;

namespace EnemyFSM.States
{
    // Terminal state. Stops and disables the agent, then destroys the GameObject after a delay.
    public class DeadState : IState<EnemyContext>
    {
        public void Enter(EnemyContext ctx)
        {
            if (ctx.Agent != null && ctx.Agent.enabled)
            {
                ctx.Agent.isStopped = true;
                ctx.Agent.enabled   = false;
            }

            Object.Destroy(ctx.Self.gameObject, ctx.Stats.DeathDestroyDelay);
        }

        public void Tick(EnemyContext ctx) { }
        public void Exit(EnemyContext ctx) { }
    }
}
