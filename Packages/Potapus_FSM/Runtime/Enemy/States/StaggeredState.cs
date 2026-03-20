using EnemyFSM.Core;

namespace EnemyFSM.States
{
    // Enemy is mid-knockback. Waits for it to finish then goes back to Chase or ReturnHome.
    public class StaggeredState : IState<EnemyContext>
    {
        public void Enter(EnemyContext ctx)
        {
            if (ctx.Knockback != null && ctx.LastHit.Force.sqrMagnitude > 0.0001f)
                ctx.Knockback.ApplyKnockback(ctx.LastHit.Force);
        }

        public void Tick(EnemyContext ctx)
        {
            if (ctx.Knockback == null || !ctx.Knockback.IsActive)
                Recover(ctx);
        }

        public void Exit(EnemyContext ctx) { }

        private static void Recover(EnemyContext ctx)
        {
            EnemyStateID next = ctx.DistanceToTarget <= ctx.Stats.AggroRange
                ? EnemyStateID.Chase
                : EnemyStateID.ReturnHome;

            ctx.StateMachine.TransitionTo(next);
        }
    }
}
