using MusicApi.Abstract;
using MusicApi.DTO.ResponseDTO;

namespace MusicApi.Utilities.Decorator
{
    public class LoggingSongOperationDecorator : ISongOperation
    {
        private readonly ISongOperation _decoratedSongOperation;

        public LoggingSongOperationDecorator(ISongOperation decoratedSongOperation)
        {
            _decoratedSongOperation = decoratedSongOperation;
        }

        public async Task<SongDTO> GetSongByIdAsync(int songId)
        {
            // Log before operation
            Console.WriteLine("Starting to retrieve song details.");

            var song = await _decoratedSongOperation.GetSongByIdAsync(songId);

            // Log after operation
            Console.WriteLine("Retrieved song details successfully.");

            return song;
        }
    }

}
