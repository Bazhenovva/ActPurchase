using System;
using System.Collections.Generic;
using System.Text;
using ActPurchase.Domain.Interfaces;

namespace ActPurchase.Dal.Contracts.Repositories
{
    /// <summary>
    /// контракт для записи данных в контекст
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// создает новую сущность в контекст 
        /// </summary>
        void Create<TEntity>(TEntity entity) where TEntity : class, IEntityWithId;

        /// <summary>
        /// обновляет сущность в контексе
        /// </summary>
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntityWithId;

        /// <summary>
        /// удаляет сущность в контексе
        /// </summary>
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityWithId;

    }
}
