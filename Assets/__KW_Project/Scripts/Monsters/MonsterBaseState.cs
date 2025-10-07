namespace KW
{
    public abstract class MonsterBaseState<T>
    {
        public abstract void EnterState(T controller);

        public abstract void UpdateState(T controller);

        public abstract void ExitState(T controller);
    }
}