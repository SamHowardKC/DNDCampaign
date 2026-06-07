using BackEnd.Entities.Auth;
using BackEnd.Entities.Campaign;
using BackEnd.Entities.Character;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterCampaign>()
                .HasKey(cc => new { cc.CharacterID, cc.CampaignID });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> User { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<CharacterCampaign> CharacterCampaign { get; set; }
        public DbSet<Character> Character { get; set; }
        public DbSet<CharacterClass> CharacterClass { get; set; }
    }
}
