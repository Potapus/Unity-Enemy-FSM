using System;
using UnityEngine;
using EnemyFSM;
using EnemyFSM.Combat;

public class EnemyHealth : MonoBehaviour, IEnemyHealth, IDamageable
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public bool IsDead { get; private set; }
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public event Action<HitData> OnHitReceived;
    public event Action<float> OnDamaged;
    public event Action OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyHit(HitData hit)
    {
        if (IsDead) return;

        OnHitReceived?.Invoke(hit);

        currentHealth -= hit.Damage;
        OnDamaged?.Invoke(hit.Damage);

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            IsDead = true;
            OnDied?.Invoke();
        }
    }
}
