using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelApp.Domain.Entities;
using ModelApp.Domain.Shareds;
using ModelApp.Infra.Mappings;
using System.Diagnostics;

namespace ModelApp.Infra.Contexts
{
    public class ModelAppContext : DbContext
    {
        #region Properties

        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Customer> Customer { get; set; }

        #endregion

        #region Constructors

        public ModelAppContext() : base() { }

        #endregion

        #region Events

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(AppSettings.SqlServerConnection)
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new UserRoleMapping());
            modelBuilder.ApplyConfiguration(new MenuMapping());
            modelBuilder.ApplyConfiguration(new CustomerMapping());
        }

        #endregion
    }
}
