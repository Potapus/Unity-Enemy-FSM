using UnityEngine;

namespace EnemyFSM.Combat
{
    // Spawns a hitbox prefab at a position, sets its size and damage, then destroys it after duration.
    // The prefab needs a BoxCollider (set to trigger) and a Hitbox component.
    public class HitboxSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _hitboxPrefab;

        public Hitbox SpawnHitbox(
            Vector3    position,
            float      radius,
            float      damage,
            Vector3    force,
            float      duration,
            GameObject attacker)
        {
            if (_hitboxPrefab is null)
            {
                Debug.LogWarning("[HitboxSpawner] hitboxPrefab is not assigned.", this);
                return null;
            }

            GameObject go = Instantiate(_hitboxPrefab, position, Quaternion.identity);

            if (go.TryGetComponent(out BoxCollider box))
            {
                box.size   = new Vector3(radius * 2f, 2f, radius * 2f);
                box.center = Vector3.zero;
            }

            Hitbox hitbox = null;
            if (go.TryGetComponent(out hitbox))
                hitbox.Initialize(new HitData(damage, force, attacker));
            else
                Debug.LogWarning("[HitboxSpawner] Spawned prefab has no Hitbox component.", this);

            Destroy(go, Mathf.Max(0.01f, duration));
            return hitbox;
        }
    }
}
