using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFData
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        
        public DbSet<UserTransactionInfo> UserTransactionInfos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTransactionInfo>()
                .HasKey(c => new { c.UserId, c.TransactionId });
            base.OnModelCreating(modelBuilder);
        }
    }
}