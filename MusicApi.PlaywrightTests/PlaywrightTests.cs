using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;

namespace MusicApi.PlaywrightTests
{
    public class PlaywrightTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly ITestOutputHelper _output;
        private readonly CustomWebApplicationFactory _factory;

        public PlaywrightTests(ITestOutputHelper output, CustomWebApplicationFactory factory)
        {
            _output = output;
            _factory = factory;
        }

        [Fact]
        public async Task TestNavigationToExampleDotCom()
        {
            // Assicurati che lo script sia nel percorso corretto e aggiornato
            string scriptPath = "\"C:\\Users\\Marco Silveri\\Desktop\\ApiMusica\\MusicApi.PlaywrightTests\\testPlaywright.js\"";

            // Configura l'avvio del processo Node.js per eseguire il test di navigazione
            var processStartInfo = new ProcessStartInfo("node", scriptPath)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string errors = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            _output.WriteLine("Output: " + output);
            _output.WriteLine("Errors: " + errors);
            _output.WriteLine("Exit Code: " + process.ExitCode);

            // Verifica che il titolo della pagina sia corretto
            Assert.Contains("Example Domain", output, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task TestPlaywrightScript()
        {
            // Avvia l'applicazione usando WebApplicationFactory
            var client = _factory.CreateClient();

            // Verifica che l'applicazione sia in esecuzione
            var response = await client.GetAsync("/Songs");
            response.EnsureSuccessStatusCode();
            _output.WriteLine("Application started successfully.");

            // Aspetta che il server sia completamente avviato
            await Task.Delay(10000);  // Aumenta questo valore se necessario

            // Aggiorna il percorso per il nuovo script CrudSong.js
            string scriptPath = "\"C:\\Users\\Marco Silveri\\Desktop\\ApiMusica\\MusicApi.PlaywrightTests\\CrudSong.js\"";

            var processStartInfo = new ProcessStartInfo("node", scriptPath)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            process.WaitForExit();

            _output.WriteLine("Output: " + output);
            _output.WriteLine("Errors: " + errors);
            _output.WriteLine("Exit Code: " + process.ExitCode);

            // Verifica che le operazioni CRUD siano state eseguite correttamente
            Assert.Contains("GET completed: Success", output, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("POST completed: Success", output, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("PUT completed: Success", output, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("DELETE completed: Success", output, StringComparison.OrdinalIgnoreCase);
        }

    }
}
