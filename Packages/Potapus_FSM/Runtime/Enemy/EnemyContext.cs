using EnemyFSM.Attack;
using EnemyFSM.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyFSM
{
    // Data bag passed into every state on Enter/Tick/Exit.
    // Health and knockback are interfaces so the package doesn't depend on your concrete MonoBehaviours.
    public class EnemyContext
    {
        public EnemyStateMachine  StateMachine { get; }
        public NavMeshAgent       Agent        { get; }
        public Transform          Self         { get; }
        public IEnemyHealth       Health       { get; }
        public IKnockbackHandler  Knockback    { get; }
        public SO_EnemyStats      Stats        { get; }
        public IEnemyAttack       Attack       { get; }
        public Vector3            HomePosition { get; }

        // Mutable — states write to these.
        public Transform Target  { get; set; }
        public HitData   LastHit { get; set; }

        // Helpers so states don't repeat the same distance math.
        public float DistanceToTarget =>
            Target != null
                ? Vector3.Distance(Self.position, Target.position)
                : float.MaxValue;

        public float DistanceFromHome =>
            Vector3.Distance(Self.position, HomePosition);

        public bool IsAgentReady =>
            Agent != null && Agent.enabled && Agent.isOnNavMesh;

        public EnemyContext(
            EnemyStateMachine stateMachine,
            NavMeshAgent      agent,
            Transform         self,
            IEnemyHealth      health,
            IKnockbackHandler knockback,
            SO_EnemyStats     stats,
            IEnemyAttack      attack,
            Vector3           homePosition)
        {
            StateMachine = stateMachine;
            Agent        = agent;
            Self         = self;
            Health       = health;
            Knockback    = knockback;
            Stats        = stats;
            Attack       = attack;
            HomePosition = homePosition;
        }
    }
}
