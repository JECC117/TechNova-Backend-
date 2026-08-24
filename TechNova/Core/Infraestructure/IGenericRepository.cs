using System.Linq.Expressions;

namespace Core.Infraestructure
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Infraestructure (Interfaces de Acceso a Datos)
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define el contrato de repositorio genérico para operaciones CRUD y consultas con 'IQueryable'.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Sigue el principio de Inversión de Dependencias (DIP) de SOLID:
    ///    El dominio (Core) define la abstracción ('IGenericRepository<T>') sin depender de Entity Framework.
    /// 2. La capa de Infraestructura implementa este contrato en 'Infraestructure/Repositories/GenericRepository.cs'.
    /// 3. Los Servicios consumen 'IGenericRepository<TEntity>' inyectado por el contenedor de dependencias.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Usar 'GetQueryable' para componer consultas LINQ diferidas con filtros y proyecciones.
    /// - Usar 'FirstOrDefaultAsyncWithIncludes' cuando se requiera eager loading con navegación de relaciones.
    /// - Usar 'AddAsync', 'Update', 'Delete' y confirmar con 'SaveAsync' o a través de 'IUnitOfWork'.
    /// =========================================================================================
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetQueryable(Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
        Task<T?> FirstOrDefaultAsyncWithIncludes(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        Task DeleteByIdAsync(object id);
        void Delete(T entity);
        void DeleteRange(List<T> entities);
        Task<bool> SaveAsync();
    }
}

