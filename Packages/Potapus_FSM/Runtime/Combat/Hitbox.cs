using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM.Combat
{
    // Trigger collider that deals damage on overlap. Spawned by HitboxSpawner and auto-destroyed.
    // Won't hit the attacker or teammates. Each target only gets hit once per spawn.
    public class Hitbox : MonoBehaviour
    {
        private HitData                       _hitData;
        private readonly HashSet<IDamageable> _hitTargets = new();

        // Fires for every unique target hit.
        public event Action OnHitConnected;

        public void Initialize(HitData hitData)
        {
            _hitData = hitData;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hitData.Attacker != null)
            {
                if (other.gameObject == _hitData.Attacker)   return;
                if (other.CompareTag(_hitData.Attacker.tag)) return;
            }

            if (!other.TryGetComponent<IDamageable>(out var target)) return;
            if (_hitTargets.Contains(target))                        return;

            _hitTargets.Add(target);
            target.ApplyHit(_hitData);
            OnHitConnected?.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (TryGetComponent<BoxCollider>(out var box))
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(box.center, box.size);
            }
        }
    }
}
