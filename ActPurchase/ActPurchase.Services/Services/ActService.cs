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
    /// Сервис для работы с актами
    /// </summary>
    public class ActService : IActService
    {
        private readonly IActRepository actRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса актов
        /// </summary>
        public ActService(IActRepository actRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.actRepository = actRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получение коллекции дто актов
        /// </summary>
        async Task<IReadOnlyCollection<ActModel>> IActService.GetAllActsAsync(CancellationToken cancellationToken)
        {
            var result = await actRepository.GetAllActsAsync(cancellationToken);
            return mapper.Map<IReadOnlyCollection<ActModel>>(result);
        }

        /// <summary>
        /// Получает акт по айди,выбрасывает исключение если не найдено
        /// </summary>
        async Task <ActModel> IActService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await actRepository.GetActByIdAsync(id, cancellationToken);
            if (result == null)
            {
                throw new EntityNotFoundException<Act>(id);
            }
            return mapper.Map<ActModel>(result);
        }

        /// <summary>
        /// Создает новый акт, если он уже существует - выбрасывает исключение
        /// </summary>
        async Task<ActModel> IActService.CreateActAsync(ActCreateModel model, CancellationToken cancellationToken)
        {
            var resultExisting = await actRepository.GetActByNumberAsync(model.Number,  cancellationToken);
            if (resultExisting != null)
            {
                throw new ActPurchaseException($"Акт с номером {model.Number} уже существует");
            }
            var entity = mapper.Map<Act>(model);
            actRepository.Create(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.Map<ActModel>(entity);
        }

        /// <summary>
        /// Обновляем акт, выбрасываем исключения при проверке на null сущность и одинаковые номера акта
        /// </summary>
        async Task IActService.UpdateActAsync(ActModel model, CancellationToken cancellationToken)
        {
            var exsistingNumber = await actRepository.GetActByNumberAsync(model.Number, cancellationToken);
            if (exsistingNumber != null && exsistingNumber.Id != model.Id)
            {
                throw new ActPurchaseException($"Акт с номером {model.Number} уже существует");
            }
            var entity = await actRepository.GetActByIdAsync(model.Id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<Act>(model.Id);
            }
            mapper.Map(model, entity);
            actRepository.Update(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаление акта по айди, ывбрасывние исключения если сущность null
        /// </summary>
        async Task IActService.DeleteActAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await actRepository.GetActByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<Act>(id);
            }
            actRepository.Delete(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }


    }
}
