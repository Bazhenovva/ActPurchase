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
    /// Сервис для работы с контрагентами
    /// </summary>
    public class CounterpartyService : ICounterpartyService
    {
        private readonly ICounterpartyRepository counterpartyRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса контрагентов
        /// </summary>
        public CounterpartyService(ICounterpartyRepository counterpartyRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.counterpartyRepository = counterpartyRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получает список всех контрагентов
        /// </summary>
        async Task<IReadOnlyCollection<CounterpartyModel>> ICounterpartyService.GetAllCounterpartiesAsync(CancellationToken cancellationToken)
        {
            var result = await counterpartyRepository.GetAllCounterpartiesAsync(cancellationToken);
            return mapper.Map<IReadOnlyCollection<CounterpartyModel>>(result);
        }

        /// <summary>
        /// Получает контрагента по идентификатору
        /// </summary>
        async Task<CounterpartyModel> ICounterpartyService.GetCounterpartyByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await counterpartyRepository.GetCounterpartyByIdAsync(id, cancellationToken);
            return result == null ? throw new EntityNotFoundException<Counterparty>(id) : mapper.Map<CounterpartyModel>(result);
        }

        /// <summary>
        /// Создаёт нового контрагента
        /// </summary>
        async Task<CounterpartyModel> ICounterpartyService.CreateCounterpartyAsync(CounterpartyCreateModel model, CancellationToken cancellationToken)
        {
            var existing = await counterpartyRepository.GetCounterpartyByInnAsync(model.Inn, cancellationToken);
            if (existing != null)
            {
                throw new ActPurchaseException($"Контрагент с ИНН {model.Inn} уже существует");
            }

            var entity = mapper.Map<Counterparty>(model);
            counterpartyRepository.Create(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<CounterpartyModel>(entity);
        }

        /// <summary>
        /// Обновляет данные контрагента
        /// </summary>
        async Task ICounterpartyService.UpdateCounterpartyAsync(CounterpartyModel model, CancellationToken cancellationToken)
        {
            var existingByInn = await counterpartyRepository.GetCounterpartyByInnAsync(model.Inn, cancellationToken);
            if (existingByInn != null && existingByInn.Id != model.Id)
            {
                throw new ActPurchaseException($"Контрагент с ИНН {model.Inn} уже существует");
            }

            var entity = await counterpartyRepository.GetCounterpartyByIdAsync(model.Id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<Counterparty>(model.Id);
            }

            mapper.Map(model, entity);
            counterpartyRepository.Update(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаляет контрагента по идентификатору
        /// </summary>
        async Task ICounterpartyService.DeleteCounterpartyAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await counterpartyRepository.GetCounterpartyByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<Counterparty>(id);
            }

            counterpartyRepository.Delete(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
