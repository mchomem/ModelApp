using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelApp.Domain.Entities;

namespace ModelApp.Infra.Mappings
{
    public class MenuMapping : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder
                .ToTable("Menu")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.Label)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(x => x.Page)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.ParentMenuId)
                .IsRequired(false);

            builder
                .Property(x => x.ImageIcon)
                .HasColumnType("varbinary")
                .IsRequired(false);

            builder
                .Property(x => x.CssFontAwesomeIcon)
                .HasMaxLength(30)
                .IsRequired(false);

            builder
                .Property(x => x.Visible)
                .IsRequired();

            builder
                .Property(x => x.Order)
                .IsRequired();

            builder
                .Property(x => x.Active)
                .IsRequired()
                .HasDefaultValueSql("1");

            #region Foreign Key mapping for Menu table.

            builder
                .HasOne(pm => pm.ParentMenu)
                .WithMany(m => m.Menus)
                .HasForeignKey(pm => pm.ParentMenuId)
                .HasConstraintName("FK_Menu_ParentMenu")
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }
    }
}
