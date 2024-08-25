namespace Bento
{
    public interface ICanSendCommand
    {
    }

    public static class CanSendCommand
    {
        public static void Send<T>(this ICanSendCommand self) where T : CommandBase, new()
        {
            var command = new T();
            command.Execute();
        }

        public static TResult Send<T, TResult>(this ICanSendCommand self) where T : CommandBase<TResult>, new()
        {
            var command = new T();
            return command.Execute();
        }
    }
}