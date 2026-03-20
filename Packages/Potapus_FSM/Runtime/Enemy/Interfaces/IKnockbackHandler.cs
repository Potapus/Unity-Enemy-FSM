using UnityEngine;

namespace EnemyFSM
{
    // Implement this on your knockback MonoBehaviour so the FSM can trigger it without a hard dependency.
    public interface IKnockbackHandler
    {
        // True while knockback is still playing out.
        bool IsActive { get; }

        void ApplyKnockback(Vector3 force);
    }
}
