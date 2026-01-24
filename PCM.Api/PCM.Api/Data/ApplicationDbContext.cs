using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Models;

namespace PCM.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Court> Courts { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        public DbSet<News> News { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Participant> Participants { get; set; }

        public DbSet<MatchScore> MatchScores { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }


        // ================= FIX CASCADE =================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Match → Member (no cascade)
            builder.Entity<Match>()
                .HasOne(m => m.Team1_Player1)
                .WithMany()
                .HasForeignKey(m => m.Team1_Player1Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Match>()
                .HasOne(m => m.Team1_Player2)
                .WithMany()
                .HasForeignKey(m => m.Team1_Player2Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Match>()
                .HasOne(m => m.Team2_Player1)
                .WithMany()
                .HasForeignKey(m => m.Team2_Player1Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Match>()
                .HasOne(m => m.Team2_Player2)
                .WithMany()
                .HasForeignKey(m => m.Team2_Player2Id)
                .OnDelete(DeleteBehavior.NoAction);


            // Participant → Member (NO CASCADE)  ⭐ FIX Ở ĐÂY
            builder.Entity<Participant>()
                .HasOne(p => p.Member)
                .WithMany()
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
