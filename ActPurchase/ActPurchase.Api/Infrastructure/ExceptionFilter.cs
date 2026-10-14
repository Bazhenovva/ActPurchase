using ActPurchase.Api.Models;
using ActPurchase.Services.Contracts.Exceptions;
using ActPurchase.Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActPurchase.Api.Infrastructure
{
    /// <summary>
    /// Обрабатывает исключения сервисов
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception as ActPurchaseException;
            if (exception == null)
            {
                return;
            }
            var message = exception.Message;

            switch (exception)
            {
                case ActPurchaseValidationException ex:
                    SetDataToContext( new BadRequestObjectResult( new ApiValidationExceptionDetail
                    {
                        Errors = ex.Errors,
                    })
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    }, context);
                    break;
                case ActPurchaseInvalidOperationException :
                    SetDataToContext( new BadRequestObjectResult( new ApiExceptionDetail
                    {
                        Message = message,
                    })
                    {
                        StatusCode = StatusCodes.Status406NotAcceptable
                    }, context);
                    break;
                case EntityNotFoundException<Act>:
                case EntityNotFoundException<ActItem>:
                case EntityNotFoundException<Counterparty>:
                case EntityNotFoundException<Director>:
                    SetDataToContext(new NotFoundObjectResult(new ApiExceptionDetail
                    {
                        Message = message,
                    }),  context);
                    break;
                default:
                    SetDataToContext(new BadRequestObjectResult(new ApiExceptionDetail
                    {
                        Message = message,
                    }) , context);
                    break;
            }

        }
        /// <summary>
        /// Определяет контекст ответа
        /// </summary>
        private static void SetDataToContext(ObjectResult data, ExceptionContext context)
        {
            context.ExceptionHandled = true;
            var response = context.HttpContext.Response;
            response.StatusCode = data.StatusCode ?? StatusCodes.Status400BadRequest;
            context.Result = data;
        }

    }
}
