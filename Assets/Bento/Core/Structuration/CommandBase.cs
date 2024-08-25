namespace Bento
{
    public abstract class CommandBase : CanGetModel
    {
        public void Execute() => OnExecute();

        protected abstract void OnExecute();
    }

    public abstract class CommandBase<TResult> : CanGetModel
    {
        public TResult Execute() => OnExecute();

        protected abstract TResult OnExecute();
    }
}