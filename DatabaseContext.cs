using Microsoft.EntityFrameworkCore;
using TestEF.Entities;

namespace TestEF
{
    public class DatabaseContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<ChatEntity> Chats { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ChatEntity>()
                .ToTable("Chats");
            modelBuilder
                .Entity<ChatEntity>()
                .HasKey(x => x.Uid);

            modelBuilder
                .Entity<MessageEntity>()
                .ToTable("Messages");
            modelBuilder
                .Entity<MessageEntity>()
                .HasKey(x => x.Uid);
            modelBuilder
                .Entity<MessageEntity>()
                .HasOne(x => x.Chat)
                .WithMany(y => y.Messages)
                .HasForeignKey(x => x.ChatUid)
                .IsRequired();
        }
    }
}
