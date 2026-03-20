namespace EnemyFSM.Core
{
    // Every state implements this. TContext is whatever data the state needs to read/write.
    public interface IState<TContext>
    {
        void Enter(TContext ctx);
        void Tick(TContext ctx);
        void Exit(TContext ctx);
    }
}
