using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Trial.Models
{
    public partial class ContosoHelpdeskContext : DbContext
    {
        public ContosoHelpdeskContext(DbContextOptions<ContosoHelpdeskContext> options) : base(options)

        {
        }

        public virtual DbSet<AppMsi> AppMsis { get; set; }
        public virtual DbSet<InstallApp> InstallApps { get; set; }
        public virtual DbSet<LocalAdmin> LocalAdmins { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<ResetPassword> ResetPasswords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppMsi>()
                .Property(e => e.AppName)
                .IsUnicode(false);

            modelBuilder.Entity<AppMsi>()
                .Property(e => e.MsiPackage)
                .IsUnicode(false);

            modelBuilder.Entity<InstallApp>()
                .Property(e => e.AppName)
                .IsUnicode(false);

            modelBuilder.Entity<InstallApp>()
                .Property(e => e.MachineName)
                .IsUnicode(false);

            modelBuilder.Entity<ResetPassword>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("ContosoHelpdeskContext");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
