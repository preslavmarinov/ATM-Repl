using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ATMContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=BankDB;Integrated Security=true;TrustServerCertificate=true;");
        }

        public DbSet<ClientEntity> Clients { get; set; }

        public DbSet<TransactionEntity> Transactions { get; set; }

        public DbSet<ATMEntity> ATMs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionEntity>()
                .HasOne(t => t.Sender)
                .WithMany(c => c.TransactionsSent)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionEntity>()
                .HasOne(t => t.Receiver)
                .WithMany(c => c.TransactionsReceived)
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientEntity>()
                .Property(c => c.Balance)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<TransactionEntity>()
               .Property(c => c.Amount)
               .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<TransactionEntity>()
               .Property(c => c.Fee)
               .HasColumnType("decimal(18, 2)");
        }
    }
}
