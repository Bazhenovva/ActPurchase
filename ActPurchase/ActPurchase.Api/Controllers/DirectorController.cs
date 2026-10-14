using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ActPurchase.Services.Contracts.Interfaces;
using ActPurchase.Api.Models;
using ActPurchase.Common;
using ActPurchase.Services.Contracts.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace ActPurchase.Api.Controllers
{
    /// <summary>
    /// класс, который принимает HTTP-запросы и возвращает HTTP-ответы
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DirectorController : ControllerBase
    {
        private readonly IDirectorService directorService;
        private readonly IMapper mapper;
        private readonly IValidateService validateService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера директоров
        /// </summary>
        public DirectorController(IDirectorService directorService, IMapper mapper, IValidateService validateService)
        {
            this.directorService = directorService;
            this.mapper = mapper;
            this.validateService = validateService;
        }

        /// <summary>
        /// Получает список директоров
        /// </summary>
        [HttpGet]
        [ProducesResponseType<IReadOnlyCollection<DirectorApiModel>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var directors = await directorService.GetAllDirectorsAsync(cancellationToken);
            return Ok(mapper.Map<IReadOnlyCollection<DirectorApiModel>>(directors));
        }

        /// <summary>
        /// Получение директора по идентификатору
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([SwaggerParameter("Идентификатор директора")] Guid id, CancellationToken cancellationToken)
        {
            var director = await directorService.GetDirectorByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<DirectorApiModel>(director));
        }

        /// <summary>
        /// Создаёт нового директора
        /// </summary>
        [HttpPost]
        [ProducesResponseType<DirectorApiModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(DirectorCreateApiModel createModel, CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<DirectorCreateModel>(createModel);
            await validateService.ValidateAsync(mapped, cancellationToken);

            var result = await directorService.CreateDirectorAsync(mapped, cancellationToken);

            return Ok(mapper.Map<DirectorApiModel>(result));
        }

        /// <summary>
        ///  Обновляет данные директора по идентификатору
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] DirectorCreateApiModel createModel,
            CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<DirectorModel>(createModel);

            mapped.Id = id;
            await validateService.ValidateAsync(mapped, cancellationToken);

            await directorService.UpdateDirectorAsync(mapped, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удаляет директора по идентификатору
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType<DirectorApiModel>(204)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await directorService.DeleteDirectorAsync(id, cancellationToken);
            return NoContent();
        }

    }

}
