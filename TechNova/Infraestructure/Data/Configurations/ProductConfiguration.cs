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
    /// Define el mapeo relacional objeto-base de datos (ORM) para la entidad 'Product' utilizando Fluent API.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. EF Core carga esta clase automáticamente durante la inicialización de 'AppDbContext'.
    /// 2. Establece el nombre de la tabla ('products' en PostgreSQL), columnas, tipos de datos,
    ///    restricciones de nulabilidad, índices y valores predeterminados.
    /// 
    /// BENEFICIOS:
    /// - Separa completamente las reglas de persistencia relacional de las clases de entidad en Core (sin Data Annotations acopladas).
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Crear un archivo '{Entidad}Configuration.cs' que implemente 'IEntityTypeConfiguration<{Entidad}>' para cada entidad.
    /// - Usar nombres en minúsculas / snake_case para compatibilidad óptima con PostgreSQL.
    /// =========================================================================================
    /// </summary>
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnName("id");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");

            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnName("description");

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("numeric(12,2)")
                .HasColumnName("price");

            builder.Property(p => p.Stock)
                .IsRequired()
                .HasColumnName("stock");

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("updated_at");

            // Índice para búsquedas rápidas por nombre
            builder.HasIndex(p => p.Name)
                .HasDatabaseName("idx_products_name");
        }
    }
}

