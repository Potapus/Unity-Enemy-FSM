using System;
using EnemyFSM.Attack;
using EnemyFSM.Combat;
using EnemyFSM.Core;
using EnemyFSM.States;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyFSM
{
    // Drop this on an enemy GameObject. Needs NavMeshAgent, EnemyHealth, and EnemyKnockback on the same object.
    // Optionally assign an IEnemyAttack MonoBehaviour (e.g. EnemyMeleeAttack) to the Attack Behaviour slot.
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyStateMachine : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private SO_EnemyStats _stats;

        [Header("Home")]
        [SerializeField] private Transform _homePoint;

        [Header("Attack Behaviour")]
        [Tooltip("Assign a MonoBehaviour that implements IEnemyAttack (e.g. EnemyMeleeAttack).")]
        [SerializeField] private MonoBehaviour _attackBehaviour;

        private readonly StateMachine<EnemyStateID, EnemyContext> _machine = new();
        private EnemyContext _ctx;

        public event Action<EnemyStateID> OnStateChanged;
        public EnemyStateID CurrentStateID => _machine.CurrentStateID;

        private void Awake()
        {
            if (_stats == null)
                Debug.LogError($"[EnemyStateMachine] {name}: SO_EnemyStats not assigned!", this);

            var agent     = GetComponent<NavMeshAgent>();
            var health    = GetComponent<IEnemyHealth>();
            var knockback = GetComponent<IKnockbackHandler>();
            var attack    = _attackBehaviour as IEnemyAttack;

            if (health == null)
                Debug.LogWarning($"[EnemyStateMachine] {name}: No IEnemyHealth component found.", this);

            if (_attackBehaviour != null && attack == null)
                Debug.LogWarning($"[EnemyStateMachine] {name}: _attackBehaviour does not implement IEnemyAttack.", this);

            Vector3 homePos = _homePoint != null ? _homePoint.position : transform.position;

            _ctx = new EnemyContext(this, agent, transform, health, knockback, _stats, attack, homePos);

            RegisterStates();

            _machine.OnStateChanged += id => OnStateChanged?.Invoke(id);

            if (health != null)
            {
                health.OnHitReceived += HandleHitReceived;
                health.OnDied        += HandleDied;
            }
        }

        private void Start()
        {
            if (_ctx.Target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    _ctx.Target = player.transform;
            }

            TransitionTo(EnemyStateID.Idle);
        }

        private void Update() => _machine.Tick(_ctx);

        private void OnDestroy()
        {
            if (_ctx?.Health == null) return;
            _ctx.Health.OnHitReceived -= HandleHitReceived;
            _ctx.Health.OnDied        -= HandleDied;
        }

        public void TransitionTo(EnemyStateID id) => _machine.TransitionTo(id, _ctx);

        public void SetTarget(Transform target) => _ctx.Target = target;

        private void RegisterStates()
        {
            _machine.RegisterState(EnemyStateID.Idle,       new IdleState());
            _machine.RegisterState(EnemyStateID.Wander,     new WanderState());
            _machine.RegisterState(EnemyStateID.ReturnHome, new ReturnHomeState());
            _machine.RegisterState(EnemyStateID.Chase,      new ChaseState());
            _machine.RegisterState(EnemyStateID.Attack,     new AttackState());
            _machine.RegisterState(EnemyStateID.Staggered,  new StaggeredState());
            _machine.RegisterState(EnemyStateID.Dead,       new DeadState());
        }

        private void HandleHitReceived(HitData hit)
        {
            if (CurrentStateID == EnemyStateID.Dead) return;

            _ctx.LastHit = hit;

            if (hit.Force.sqrMagnitude > 0.0001f && CurrentStateID != EnemyStateID.Staggered)
                TransitionTo(EnemyStateID.Staggered);
            else if (CurrentStateID == EnemyStateID.Idle || CurrentStateID == EnemyStateID.Wander)
                TransitionTo(EnemyStateID.Chase);
        }

        private void HandleDied() => TransitionTo(EnemyStateID.Dead);
    }
}
