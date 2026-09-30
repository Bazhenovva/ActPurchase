using ActPurchase.Services.Contracts.DTO;
using FluentValidation;

namespace ActPurchase.Services.Validators.Base
{
    /// <summary>
    /// Базовый валидатор моделей позиций актов
    /// </summary>
    public abstract class BaseActItemValidator<T> : AbstractValidator<T> where T : ActItemCreateModel
    {
        /// <summary>
        /// Инициализирует валидатор и настраивает общие правила
        /// </summary>
        protected BaseActItemValidator()
        {
            RuleFor(x => x.ActId)
                .NotEmpty()
                .WithMessage("Идентификатор акта обязателен");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Наименование обязательно")
                .MaximumLength(15)
                .WithMessage("Наименование не более 15 символов");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Количество должно быть больше 0");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Цена должна быть больше 0");
        }
    }
}
