using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Infraestructure
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Infraestructure
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define el contrato para el patrón Unit of Work (Unidad de Trabajo) y gestión de transacciones.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Permite agrupar múltiples operaciones sobre distintos repositorios en una única transacción atómica.
    /// 2. Si alguna operación falla dentro de la transacción, se llama a 'RollbackAsync()' para revertir todo.
    /// 3. Si todas son exitosas, se ejecuta 'CommitAsync()' persistiendo los cambios de forma segura en PostgreSQL.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Usar 'IUnitOfWork' en operaciones complejas que afecten varias tablas simultáneamente (ej. Crear Pedido y descontar Stock).
    /// =========================================================================================
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        IExecutionStrategy CreateExecutionStrategy();
        Task SaveChangesAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}

