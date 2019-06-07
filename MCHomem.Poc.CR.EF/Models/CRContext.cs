using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHomem.Poc.CR.EF.Models
{
    public class CRContext : DbContext
    {
        public CRContext() : base("name=CRDataBase") { }

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
