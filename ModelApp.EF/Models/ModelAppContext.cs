using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ModelApp.EF.Models
{
    public class ModelAppContext : DbContext
    {
        public ModelAppContext() : base("name=ModelApp") { }

        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            dbModelBuilder
                .Entity<Menu>()
                .HasOptional(m => m.ParentMenu)
                .WithMany()
                .HasForeignKey(pm => pm.ParentMenuID);

            base.OnModelCreating(dbModelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }
    }
}
