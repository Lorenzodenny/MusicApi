using Microsoft.EntityFrameworkCore;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.Model;
using MusicApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApi.Tests.TestConIClassFixture
{
    public class DatabaseFixture : IDisposable
    {
        public MusicApiContext Context { get; private set; }
        public DatabaseFixture()
        {
            // Configura e inizializza il DbContext qui, potresti voler usare un database InMemory
            var options = new DbContextOptionsBuilder<MusicApiContext>()
                          .UseInMemoryDatabase(databaseName: "TestDb")
                          .Options;
            Context = new MusicApiContext(options);



            // Aggiungi dati di test con album
            var album = new Album { Name = "Test Album", Genre = "Test Genre" };
            Context.Albums.Add(album);
            Context.Songs.AddRange(
                new Song { Name = "Test Song 1", Year = 2020, Album = album },
                new Song { Name = "Test Song 2", Year = 2021, Album = album });
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }

}
