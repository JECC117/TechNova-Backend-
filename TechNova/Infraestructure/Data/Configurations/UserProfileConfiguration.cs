using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Configurations
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Data -> Configurations
    /// =========================================================================================
    /// PROPÓSITO:
    /// Configuración Fluent API para el mapeo de la entidad 'UserProfile' a la tabla 'user_profiles' en PostgreSQL.
    /// =========================================================================================
    /// </summary>
    internal class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("user_profiles");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .IsRequired()
                .HasColumnName("id");

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("first_name");

            builder.Property(u => u.LastName)
                .IsRequired(false)
                .HasMaxLength(100)
                .HasColumnName("last_name");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("email");

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("password_hash");

            builder.Property(u => u.PhoneNumber)
                .IsRequired(false)
                .HasMaxLength(20)
                .HasColumnName("phone_number");

            builder.Property(u => u.ShippingAddress)
                .IsRequired(false)
                .HasMaxLength(300)
                .HasColumnName("shipping_address");

            builder.Property(u => u.Avatar)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnName("avatar");

            builder.Property(u => u.RoleId)
                .IsRequired()
                .HasColumnName("role_id");

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");

            builder.Property(u => u.UpdatedAt)
                .IsRequired(false)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("updated_at");

            // Índice único para email
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("idx_user_profiles_email");

            // Relación con Role
            builder.HasOne(u => u.Role)
                .WithMany(r => r.UserProfiles)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

