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
    public class ActItemController : ControllerBase
    {
        private readonly IActItemService actItemService;
        private readonly IMapper mapper;
        private readonly IValidateService validateService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера позиций
        /// </summary>
        public ActItemController(IActItemService actItemService, IMapper mapper, IValidateService validateService)
        {
            this.actItemService = actItemService;
            this.mapper = mapper;
            this.validateService = validateService;
        }

        /// <summary>
        /// Получает список позиций указанного акта
        /// </summary>
        [HttpGet]
        [ProducesResponseType<IReadOnlyCollection<ActItemApiModel>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] Guid actId, CancellationToken cancellationToken)
        {
            var items = await actItemService.GetAllActItemsAsync(actId, cancellationToken);
            return Ok(mapper.Map<IReadOnlyCollection<ActItemApiModel>>(items));
        }

        /// <summary>
        /// Получение позиции по идентификатору
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([SwaggerParameter("Идентификатор позиции")] Guid id, CancellationToken cancellationToken)
        {
            var item = await actItemService.GetActItemByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<ActItemApiModel>(item));
        }

        /// <summary>
        /// Создаёт новую позицию
        /// </summary>
        [HttpPost]
        [ProducesResponseType<ActItemApiModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(ActItemCreateApiModel createModel, CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<ActItemCreateModel>(createModel);
            await validateService.ValidateAsync(mapped, cancellationToken);

            var result = await actItemService.CreateActItemAsync(mapped, cancellationToken);

            return Ok(mapper.Map<ActItemApiModel>(result));
        }

        /// <summary>
        ///  Обновляет данные позиции по идентификатору
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ActItemCreateApiModel createModel,
            CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<ActItemModel>(createModel);

            mapped.Id = id;
            mapped.Sum = mapped.Quantity * mapped.Price;
            await validateService.ValidateAsync(mapped, cancellationToken);

            await actItemService.UpdateActItemAsync(mapped, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удаляет позицию по идентификатору
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType<ActItemApiModel>(204)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await actItemService.DeleteActItemAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
