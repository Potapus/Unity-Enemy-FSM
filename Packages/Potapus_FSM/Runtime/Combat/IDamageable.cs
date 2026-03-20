namespace EnemyFSM.Combat
{
    // Anything that can take a hit implements this. The hitbox calls ApplyHit when it touches something.
    public interface IDamageable
    {
        void ApplyHit(HitData hit);
    }
}
