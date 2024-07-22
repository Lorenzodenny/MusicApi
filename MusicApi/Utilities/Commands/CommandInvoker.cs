using MusicApi.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApi.Utilities.Commands
{
    public class CommandInvoker : ICommandInvoker
    {
        private readonly Queue<ICommand> _commands = new Queue<ICommand>();

        public void AddCommand(ICommand command)
        {
            _commands.Enqueue(command);
        }

        public async Task ExecuteCommandsAsync()
        {
            while (_commands.Count > 0)
            {
                var command = _commands.Dequeue();
                await command.ExecuteAsync();
            }
        }
    }
}
