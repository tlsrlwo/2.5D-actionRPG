namespace KW
{
    public abstract class MovementBaseState
    {
        public abstract void EnterState(PlayerMovement movement);

        public abstract void UpdateState(PlayerMovement movement);

        public abstract void ExitState(PlayerMovement movement);
    }
}