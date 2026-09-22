using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Domain.Entities;
using ActPurchase.Entites.Configurations;
using ActPurchase.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ActPurchase.Context
{
    /// <summary>
    /// Контекст базы данных ActPurchase
    /// </summary>
    public class ActPurchaseContext : DbContext, IReader, IWriter, IUnitOfWork
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ActPurchaseContext"/>
        /// </summary>
        public ActPurchaseContext(DbContextOptions<ActPurchaseContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
        /// <summary>
        /// Таблица актов
        /// </summary>
        public DbSet<Act> Acts { get; set; }
        /// <summary>
        /// Таблица позиций актов
        /// </summary>
        public DbSet<ActItem> ActItems { get; set; }
        /// <summary>
        /// Таблица контрагентов
        /// </summary>
        public DbSet<Counterparty> Counterparties { get; set; }
        /// <summary>
        /// Таблица директоров
        /// </summary>
        public DbSet<Director> Directors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActConfiguration).Assembly);
        }

        /// <summary>
        /// Возвращает запрос на чтение сущностей указанного типа без отслеживания
        /// </summary>
        IQueryable<TEntity> IReader.Read<TEntity>() => base.Set<TEntity>().AsNoTracking();

        /// <summary>
        /// Помечает сущность как новую для последующего сохранения в БД
        /// </summary>
        void IWriter.Create<TEntity>(TEntity entity) => base.Entry(entity).State = EntityState.Added;

        /// <summary>
        /// Помечает сущность как изменённую для последующего сохранения в БД
        /// </summary>
        void IWriter.Update<TEntity>(TEntity entity) => base.Entry(entity).State = EntityState.Modified;

        /// <summary>
        /// Помечает сущность как удалённую для последующего сохранения в БД
        /// </summary>
        void IWriter.Delete<TEntity>(TEntity entity) => base.Entry(entity).State = EntityState.Deleted;

        /// <summary>
        /// Сохраняет все изменения в БД и отсоединяет сущности от контекста
        /// </summary>
        async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        {
            var count = await base.SaveChangesAsync(cancellationToken);

            foreach (var entry in base.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }

            return count;
        }
    }
}
