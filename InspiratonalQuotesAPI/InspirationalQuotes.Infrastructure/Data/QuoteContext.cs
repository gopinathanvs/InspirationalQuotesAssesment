using Microsoft.EntityFrameworkCore;
using InspirationalQuotes.Domain.Entities;

namespace InspirationalQuotes.Infrastructure.Data
{
    public class QuoteContext : DbContext
    {
        public QuoteContext(DbContextOptions<QuoteContext> options)
            : base(options)
        {
        }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuoteTag> QuoteTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuoteTag>()
                .HasKey(qt => new { qt.QuoteId, qt.TagId });

            modelBuilder.Entity<QuoteTag>()
                .HasOne(qt => qt.Quote)
                .WithMany(q => q.QuoteTags)
                .HasForeignKey(qt => qt.QuoteId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a quote is deleted

            modelBuilder.Entity<QuoteTag>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuoteTags)
                .HasForeignKey(qt => qt.TagId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a tag is deleted

            // Unique constraint for TagName
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.TagName)
                .IsUnique();
        }
    }
}
