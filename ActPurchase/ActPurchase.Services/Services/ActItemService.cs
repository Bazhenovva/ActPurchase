using ActPurchase.Domain.Entites;
using ActPurchase.Dal.Contracts.Repositories;
using ActPurchase.Repositories.Contracts;
using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Contracts.Exceptions;
using ActPurchase.Services.Contracts.Interfaces;
using AutoMapper;

namespace ActPurchase.Services.Services
{
    /// <summary>
    /// Сервис для работы с позициями актов
    /// </summary>
    public class ActItemService : IActItemService
    {
        private readonly IActItemRepository actItemRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса позиций актов
        /// </summary>
        public ActItemService(IActItemRepository actItemRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.actItemRepository = actItemRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получает список позиций указанного акта
        /// </summary>
        async Task<IReadOnlyCollection<ActItemModel>> IActItemService.GetAllActItemsAsync(Guid actId, CancellationToken cancellationToken)
        {
            var result = await actItemRepository.GetAllActItemsAsync(actId, cancellationToken);
            return mapper.Map<IReadOnlyCollection<ActItemModel>>(result);
        }

        /// <summary>
        /// Получает позицию по идентификатору
        /// </summary>
        async Task<ActItemModel> IActItemService.GetActItemByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await actItemRepository.GetActItemByIdAsync(id, cancellationToken);
            return result == null ? throw new EntityNotFoundException<ActItem>(id) : mapper.Map<ActItemModel>(result);
        }

        /// <summary>
        /// Создаёт новую позицию
        /// </summary>
        async Task<ActItemModel> IActItemService.CreateActItemAsync(ActItemCreateModel model, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<ActItem>(model);
            entity.Sum = entity.Quantity * entity.Price;

            actItemRepository.Create(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ActItemModel>(entity);
        }

        /// <summary>
        /// Обновляет данные позиции
        /// </summary>
        async Task IActItemService.UpdateActItemAsync(ActItemModel model, CancellationToken cancellationToken)
        {
            var entity = await actItemRepository.GetActItemByIdAsync(model.Id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<ActItem>(model.Id);
            }

            mapper.Map(model, entity);
            entity.Sum = entity.Quantity * entity.Price;

            actItemRepository.Update(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаляет позицию по идентификатору
        /// </summary>
        async Task IActItemService.DeleteActItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await actItemRepository.GetActItemByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<ActItem>(id);
            }

            actItemRepository.Delete(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
