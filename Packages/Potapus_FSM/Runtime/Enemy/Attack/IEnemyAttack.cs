namespace EnemyFSM.Attack
{
    // Implement this to create a new attack type (melee, ranged, etc.) without touching any state.
    // Assign the MonoBehaviour to EnemyStateMachine's Attack Behaviour slot.
    public interface IEnemyAttack
    {
        // True when the cooldown is done and the attack can fire.
        bool IsReady { get; }

        // AttackState calls this every frame IsReady is true.
        void Execute(EnemyContext ctx);
    }
}
