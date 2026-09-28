using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Domain.Interfaces;

namespace ActPurchase.Context.Repositories
{
    /// <summary>
    /// Базовый класс репозитория записи данных с аудитом
    /// </summary>
    public abstract class BaseWriteRepository<T> : IBaseWriteRepository<T> where T : class, IEntityWithId
    {
        private readonly IDbWriterContext writerContext;
        /// <summary>
        /// Конструктор, инициализирует экземпляр
        /// </summary>
        protected BaseWriteRepository(IDbWriterContext writerContext)
        {
            this.writerContext = writerContext;
        }

        /// <summary>
        /// Создаёт новую сущность с заполнением аудита создания
        /// </summary>
        public void Create(T entity)
        {
            if (entity is IEntityWithId entityWithId && entityWithId.Id == Guid.Empty)
            {
                entityWithId.Id = Guid.NewGuid();
            }
            AuditForCreate(entity);
            AuditForUpdate(entity);
            writerContext.Writer.Create(entity);
        }

        /// <summary>
        /// Обновляет сущность с заполнением аудита обновления
        /// </summary>
        public void Update(T entity)
        {
            AuditForUpdate(entity);
            writerContext.Writer.Update(entity);
        }

        /// <summary>
        /// Удаляет сущность
        /// </summary>
        public void Delete(T entity)
        {
            if (entity is IEntityAuditDeletedAt)
            {
                AuditForUpdate(entity);
                AuditForDelete(entity);
                writerContext.Writer.Update(entity);
            }
            else
            {
                writerContext.Writer.Delete(entity);
            }
        }

        /// <summary>
        /// Заполняет поля аудита создания 
        /// </summary>
        private void AuditForCreate(T entity)
        {
            if (entity is IEntityAuditCreated auditCreated)
            {
                auditCreated.CreatedAt = writerContext.DateTimeProvider.UtcNow;
                auditCreated.CreatedBy = writerContext.IdentityProvider.Name;
            }
        }

        /// <summary>
        /// Заполняет поля аудита обновления 
        /// </summary>
        private void AuditForUpdate(T entity)
        {
            if (entity is IEntityAuditUpdated auditUpdated)
            {
                auditUpdated.UpdatedAt = writerContext.DateTimeProvider.UtcNow;
                auditUpdated.UpdatedBy = writerContext.IdentityProvider.Name;
            }
        }

        /// <summary>
        /// Заполняет поля аудита удаления
        /// </summary>
        private void AuditForDelete(T entity)
        {
            if (entity is IEntityAuditDeletedAt auditDeleted)
            {
                auditDeleted.DeletedAt = writerContext.DateTimeProvider.UtcNow;
                auditDeleted.DeletedBy = writerContext.IdentityProvider.Name;
            }
        }
    }
}
