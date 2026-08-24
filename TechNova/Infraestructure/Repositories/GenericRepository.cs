using Core.Infraestructure;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Repositories
    /// =========================================================================================
    /// PROPÓSITO:
    /// Implementación genérica reutilizable de acceso a datos para cualquier entidad del dominio.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Implementa 'IGenericRepository<T>' utilizando el 'DbSet<T>' de 'AppDbContext'.
    /// 2. Proporciona métodos estándar para CRUD (Add, Update, Delete) y consultas LINQ asíncronas.
    /// 3. 'GetQueryable' permite a los servicios encadenar filtros '.Where(...)', ordenamientos '.OrderBy(...)',
    ///    proyecciones '.Select(...)' y paginaciones '.Skip().Take()' de forma diferida antes de ejecutar el SQL.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Para operaciones CRUD estándar de una entidad, basta con inyectar 'IGenericRepository<MiEntidad>'.
    /// - Si la entidad requiere operaciones complejas de base de datos o llamadas a funciones PostgreSQL,
    ///   crear un repositorio especializado heredando de 'GenericRepository<MiEntidad>' (como 'ProductRepository').
    /// =========================================================================================
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        internal DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetQueryable(Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
        {
            IQueryable<T> query = _dbSet;
            return includeFunc != null ? includeFunc(query) : query;
        }

        public async Task<T?> FirstOrDefaultAsyncWithIncludes(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
        {
            IQueryable<T> query = _dbSet;

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public async Task DeleteByIdAsync(object id)
        {
            T? entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

