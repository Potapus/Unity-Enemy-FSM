using UnityEngine;

namespace EnemyFSM
{
    // All the tuning values for an enemy type. Right-click in Project → Create → EnemyFSM → Enemy Stats.
    [CreateAssetMenu(fileName = "SO_EnemyStats", menuName = "EnemyFSM/Enemy Stats")]
    public class SO_EnemyStats : ScriptableObject
    {
        [Header("Health")]
        [Min(1f)] public float MaxHealth         = 100f;
        [Min(0f)] public float DeathDestroyDelay = 2f;

        [Header("Aggro")]
        [Min(0f)] public float AggroRange    = 10f;
        [Min(0f)] public float AttackRange   = 2f;
        [Min(0f)] public float LoseAggroRange = 12f;
        [Min(0f)] public float LoseAggroAfter = 3f;

        [Header("Home")]
        [Min(0f)] public float HomeRadius = 15f;

        [Header("Wander")]
        [Min(0f)] public float WanderRadius               = 8f;
        [Min(0f)] public float MinWanderDelay             = 1f;
        [Min(0f)] public float MaxWanderDelay             = 3f;
        [Min(0f)] public float DestinationReachedDistance = 1.2f;
        [Min(1f)] public float NavMeshSampleRadius        = 8f;
    }
}
