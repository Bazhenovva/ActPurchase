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
    /// Сервис для работы с директорами
    /// </summary>
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository directorRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса директоров
        /// </summary>
        public DirectorService(IDirectorRepository directorRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.directorRepository = directorRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получает список всех директоров
        /// </summary>
        async Task<IReadOnlyCollection<DirectorModel>> IDirectorService.GetAllDirectorsAsync(CancellationToken cancellationToken)
        {
            var result = await directorRepository.GetAllDirectorsAsync(cancellationToken);
            return mapper.Map<IReadOnlyCollection<DirectorModel>>(result);
        }

        /// <summary>
        /// Получает директора по идентификатору
        /// </summary>
        async Task<DirectorModel> IDirectorService.GetDirectorByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await directorRepository.GetDirectorByIdAsync(id, cancellationToken);
            return result == null ? throw new EntityNotFoundException<Director>(id) : mapper.Map<DirectorModel>(result);
        }

        /// <summary>
        /// Создаёт нового директора
        /// </summary>
        async Task<DirectorModel> IDirectorService.CreateDirectorAsync(DirectorCreateModel model, CancellationToken cancellationToken)
        {
            var existing = await directorRepository.GetDirectorByFullNameAsync(model.LastName, model.FirstName, cancellationToken);
            if (existing != null)
            {
                throw new ActPurchaseException($"Директор {model.LastName} {model.FirstName} уже существует");
            }

            var entity = mapper.Map<Director>(model);
            directorRepository.Create(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<DirectorModel>(entity);
        }

        /// <summary>
        /// Обновляет данные директора
        /// </summary>
        async Task IDirectorService.UpdateDirectorAsync(DirectorModel model, CancellationToken cancellationToken)
        {
            var existingByFullName = await directorRepository.GetDirectorByFullNameAsync(model.LastName, model.FirstName, cancellationToken);
            if (existingByFullName != null && existingByFullName.Id != model.Id)
            {
                throw new ActPurchaseException($"Директор {model.LastName} {model.FirstName} уже существует");
            }

            var entity = await directorRepository.GetDirectorByIdAsync(model.Id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<Director>(model.Id);
            }

            mapper.Map(model, entity);
            directorRepository.Update(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаляет директора по идентификатору
        /// </summary>
        async Task IDirectorService.DeleteDirectorAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await directorRepository.GetDirectorByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<Director>(id);
            }

            directorRepository.Delete(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
