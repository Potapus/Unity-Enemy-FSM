using EnemyFSM.Combat;
using UnityEngine;

namespace EnemyFSM.Attack
{
    // Spawns a hitbox in front of the enemy on attack. All values are tunable in the Inspector.
    // Swap with a different IEnemyAttack (e.g. ranged) without changing any FSM code.
    public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
    {
        [Header("Hitbox")]
        [SerializeField] private HitboxSpawner _hitboxSpawner;
        [Tooltip("How far in front of the enemy the hitbox spawns.")]
        [SerializeField] private float _hitboxOffset   = 1f;
        [SerializeField] private float _hitboxRadius   = 1f;
        [SerializeField] private float _hitboxDuration = 0.15f;

        [Header("Damage")]
        [SerializeField] private float _damage         = 20f;
        [SerializeField] private float _knockbackForce = 5f;
        [SerializeField] private float _knockUpForce   = 2f;

        [Header("Cooldown")]
        [SerializeField] private float _cooldown = 1.5f;

        private float _nextAttackTime;

        public bool IsReady => Time.time >= _nextAttackTime;

        public void Execute(EnemyContext ctx)
        {
            if (_hitboxSpawner is null)
            {
                Debug.LogWarning($"[EnemyMeleeAttack] {name}: HitboxSpawner not assigned.", this);
                return;
            }

            _nextAttackTime = Time.time + _cooldown;

            Vector3 origin = ctx.Self.position + ctx.Self.forward * _hitboxOffset;
            Vector3 force  = ctx.Self.forward * _knockbackForce + Vector3.up * _knockUpForce;

            _hitboxSpawner.SpawnHitbox(origin, _hitboxRadius, _damage, force, _hitboxDuration, ctx.Self.gameObject);
        }
    }
}
