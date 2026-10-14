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
    public class ActController : ControllerBase
    {
        private readonly IActService actService;
        private readonly IMapper mapper;
        private readonly IValidateService validateService;
        private readonly IActExcelService actExcelService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера актов
        /// </summary>
        public ActController(IActService actService, IMapper mapper, IValidateService validateService, IActExcelService actExcelService)
        {
            this.actService = actService;
            this.mapper = mapper;
            this.validateService = validateService;
            this.actExcelService = actExcelService;
        }

        /// <summary>
        /// Получает список актов
        /// </summary>
        [HttpGet]
        [ProducesResponseType<IReadOnlyCollection<ActApiModel>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var acts = await actService.GetAllActsAsync(cancellationToken);
            return Ok(mapper.Map<IReadOnlyCollection<ActApiModel>>(acts));
        }

        /// <summary>
        /// Получение дисциплины по идентификатору
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([SwaggerParameter("Идентификатор акта")] Guid id, CancellationToken cancellationToken)
        {
            var act = await actService.GetByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<ActApiModel>(act));
        }

        /// <summary>
        /// Создаёт новый акт
        /// </summary>
        [HttpPost]
        [ProducesResponseType<ActApiModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(ActCreateApiModel createModel, CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<ActCreateModel>(createModel);
            await validateService.ValidateAsync(mapped, cancellationToken);

            var result = await actService.CreateActAsync(mapped, cancellationToken);

            return Ok(mapper.Map<ActApiModel>(result));
        }

        /// <summary>
        ///  Обновляет данные акта по идентификатору
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ActCreateApiModel createModel,
            CancellationToken cancellationToken)
        {
            var mapped = mapper.Map<ActModel>(createModel);

            mapped.Id = id;
            await validateService.ValidateAsync(mapped, cancellationToken);

            await actService.UpdateActAsync(mapped, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удаляет акт по идентификатору
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType<ActApiModel>(204)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await actService.DeleteActAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Получить Excel-файл по акту
        /// </summary>
        [HttpGet("{id:guid}/excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActExcel(Guid id, CancellationToken cancellationToken)
        {
            var act = await actService.GetByIdAsync(id, cancellationToken);
            var stream = await actExcelService.GenerateActExcelAsync(act, cancellationToken);
            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"act-{id}.xlsx");
        }
    }
}
