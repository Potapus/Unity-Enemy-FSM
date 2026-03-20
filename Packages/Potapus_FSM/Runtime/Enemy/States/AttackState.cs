using EnemyFSM.Core;
using UnityEngine;

namespace EnemyFSM.States
{
    // Stops, faces the target, and fires the attack whenever it's ready.
    // Goes back to Chase if the target moves out of attack range.
    public class AttackState : IState<EnemyContext>
    {
        public void Enter(EnemyContext ctx)
        {
            if (ctx.IsAgentReady)
                ctx.Agent.isStopped = true;

            FaceTarget(ctx);
        }

        public void Tick(EnemyContext ctx)
        {
            if (ctx.DistanceToTarget > ctx.Stats.AttackRange)
            {
                ctx.StateMachine.TransitionTo(EnemyStateID.Chase);
                return;
            }

            FaceTarget(ctx);

            if (ctx.Attack != null && ctx.Attack.IsReady)
                ctx.Attack.Execute(ctx);
        }

        public void Exit(EnemyContext ctx)
        {
            if (ctx.IsAgentReady)
                ctx.Agent.isStopped = false;
        }

        private static void FaceTarget(EnemyContext ctx)
        {
            if (ctx.Target is null) return;

            Vector3 dir = (ctx.Target.position - ctx.Self.position).normalized;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.001f)
                ctx.Self.rotation = Quaternion.LookRotation(dir);
        }
    }
}
