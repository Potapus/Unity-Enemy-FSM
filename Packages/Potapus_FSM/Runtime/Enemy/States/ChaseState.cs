using EnemyFSM.Core;
using UnityEngine;

namespace EnemyFSM.States
{
    // Follows the target at full speed. Attacks when close enough.
    // Gives up and goes home if the player stays out of range too long or the enemy drifts too far.
    public class ChaseState : IState<EnemyContext>
    {
        private float _timeOutOfRange;

        public void Enter(EnemyContext ctx)
        {
            _timeOutOfRange = 0f;

            if (!ctx.IsAgentReady) return;

            ctx.Agent.isStopped        = false;
            ctx.Agent.stoppingDistance = ctx.Stats.AttackRange;
        }

        public void Tick(EnemyContext ctx)
        {
            if (!ctx.IsAgentReady) return;

            if (ctx.DistanceFromHome > ctx.Stats.HomeRadius)
            {
                ctx.StateMachine.TransitionTo(EnemyStateID.ReturnHome);
                return;
            }

            float dist = ctx.DistanceToTarget;

            if (dist <= ctx.Stats.AttackRange)
            {
                ctx.StateMachine.TransitionTo(EnemyStateID.Attack);
                return;
            }

            if (ctx.Target != null)
                ctx.Agent.SetDestination(ctx.Target.position);

            if (dist > ctx.Stats.LoseAggroRange)
                _timeOutOfRange += Time.deltaTime;
            else
                _timeOutOfRange = 0f;

            if (_timeOutOfRange >= ctx.Stats.LoseAggroAfter)
                ctx.StateMachine.TransitionTo(EnemyStateID.ReturnHome);
        }

        public void Exit(EnemyContext ctx)
        {
            if (ctx.IsAgentReady)
                ctx.Agent.stoppingDistance = 0f;
        }
    }
}
