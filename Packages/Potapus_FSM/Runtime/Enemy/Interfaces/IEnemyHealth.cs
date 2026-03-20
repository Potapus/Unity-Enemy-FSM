using System;
using EnemyFSM.Combat;

namespace EnemyFSM
{
    /// <summary>
    /// Abstracts the health/damage component so the FSM package has no hard
    /// dependency on any specific Enemy MonoBehaviour.
    ///
    /// In your project: make your Enemy class implement this interface.
    ///   public class Enemy : MonoBehaviour, IEnemyHealth { ... }
    /// </summary>
    public interface IEnemyHealth
    {
        bool  IsDead        { get; }
        float CurrentHealth { get; }
        float MaxHealth     { get; }

        event Action<HitData> OnHitReceived;
        event Action<float>   OnDamaged;
        event Action          OnDied;
    }
}
