using Core.Infraestructure;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infraestructure.Persistence
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Persistence
    /// =========================================================================================
    /// PROPÓSITO:
    /// Implementación concreta del patrón Unit of Work sobre Entity Framework Core y PostgreSQL.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Coordina el trabajo de múltiples repositorios compartiendo una única instancia de 'AppDbContext'.
    /// 2. 'SaveChangesAsync()' persiste todos los cambios rastreados en una sola transacción implícita.
    /// 3. 'BeginTransactionAsync()', 'CommitAsync()' y 'RollbackAsync()' proporcionan control explícito
    ///    para transacciones distribuidas o flujos de negocio complejos con múltiples pasos.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Inyectar 'IUnitOfWork' en los servicios de negocio cuando se requiera persistir datos o controlar transacciones.
    /// =========================================================================================
    /// </summary>
    public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        private readonly AppDbContext _dbContext = dbContext;
        private IDbContextTransaction? _transaction;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
            return _transaction;
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _dbContext.Database.CreateExecutionStrategy();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

