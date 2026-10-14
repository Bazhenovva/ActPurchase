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
    public class CounterpartyController : ControllerBase
    {
        private readonly ICounterpartyService counterpartyService;
        private readonly IMapper mapper;
        private readonly IValidateService validateService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера контрагентов
        /// </summary>
        public CounterpartyController(ICounterpartyService counterpartyService, IMapper mapper, IValidateService validateService)
        {
            this.counterpartyService = counterpartyService;
            this.mapper = mapper;
            this.validateService = validateService;
        }

        /// <summary>
        /// Получает список контрагентов
        /// </summary>
        [HttpGet]
        [ProducesResponseType<IReadOnlyCollection<CounterpartyApiModel>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var counterparties = await counterpartyService.GetAllCounterpartiesAsync(cancellationToken);
            return Ok(mapper.Map<IReadOnlyCollection<CounterpartyApiModel>>(counterparties));
        }

        /// <summary>
        /// Получение контрагента по идентификатору
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([SwaggerParameter("Идентификатор контрагента")] Guid id, CancellationToken cancellationToken)
        {
            var counterparty = await counterpartyService.GetCounterpartyByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<CounterpartyApiModel>(counterparty));
        }

        /// <summary>
        /// Создаёт нового контрагента
        /// </summary>
        [HttpPost]
        [ProducesResponseType<CounterpartyApiModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(CounterpartyCreateApiModel createModel, CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<CounterpartyCreateModel>(createModel);
            await validateService.ValidateAsync(mapped, cancellationToken);

            var result = await counterpartyService.CreateCounterpartyAsync(mapped, cancellationToken);

            return Ok(mapper.Map<CounterpartyApiModel>(result));
        }

        /// <summary>
        ///  Обновляет данные контрагента по идентификатору
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CounterpartyCreateApiModel createModel,
            CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<CounterpartyModel>(createModel);

            mapped.Id = id;
            await validateService.ValidateAsync(mapped, cancellationToken);

            await counterpartyService.UpdateCounterpartyAsync(mapped, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удаляет контрагента по идентификатору
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType<CounterpartyApiModel>(204)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await counterpartyService.DeleteCounterpartyAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
