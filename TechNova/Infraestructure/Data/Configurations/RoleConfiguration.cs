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
    /// Configuración Fluent API para el mapeo de la entidad 'Role' a la tabla 'roles' en PostgreSQL.
    /// =========================================================================================
    /// </summary>
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name");

            builder.Property(r => r.Description)
                .IsRequired(false)
                .HasMaxLength(250)
                .HasColumnName("description");

            builder.HasIndex(r => r.Name)
                .IsUnique()
                .HasDatabaseName("idx_roles_name");
        }
    }
}

