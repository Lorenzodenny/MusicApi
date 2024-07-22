namespace MusicApi.Abstract
{
    public interface ICommandInvoker
    {
        void AddCommand(ICommand command);
        Task ExecuteCommandsAsync();
    }
}
