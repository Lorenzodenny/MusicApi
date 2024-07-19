using Microsoft.EntityFrameworkCore;
using MusicApi.Model;

namespace MusicApi.DataAccessLayer
{
    public class MusicApiContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }

        public MusicApiContext(DbContextOptions<MusicApiContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione della relazione Artist-Album
            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Albums)
                .WithOne(b => b.Artist)
                .HasForeignKey(b => b.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);  // Opzionale: specifica il comportamento di eliminazione

            // Configurazione della relazione Album-Song
            modelBuilder.Entity<Album>()
                .HasMany(a => a.Songs)
                .WithOne(s => s.Album)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);  // Opzionale: specifica il comportamento di eliminazione

        }
    }
}
