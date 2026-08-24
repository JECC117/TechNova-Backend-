using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infraestructure.Data
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Data
    /// =========================================================================================
    /// PROPÓSITO:
    /// Contexto central de base de datos de Entity Framework Core para TechNova.
    /// =========================================================================================
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<UserProfile> UserProfiles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica automáticamente todas las clases IEntityTypeConfiguration<T> de este ensamblado
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
