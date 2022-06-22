using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelApp.Domain.Entities;

namespace ModelApp.Infra.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable("User")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(x => x.Login)
                .IsUnique();

            builder
                .Property(x => x.Login)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.UserRoleId)
                .IsRequired();

            builder
                .Property(x => x.Password)
                .HasMaxLength(500)
                .IsRequired();

            builder
                .Property(x => x.SecretPhrase)
                .HasMaxLength(500)
                .IsRequired(false);

            builder
                .Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Avatar)
                .HasColumnType("varbinary")
                .IsRequired(false);

            builder
                .Property(x => x.Active)
                .IsRequired()
                .HasDefaultValueSql("1");

            #region Foreign Key mapping for User table.

            builder
                .HasOne(u => u.UserRole)
                .WithMany(ur => ur.Users)
                .HasForeignKey(u => u.UserRoleId)
                .HasConstraintName("FK_User_UserRole");

            #endregion
        }
    }
}
