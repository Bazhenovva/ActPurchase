using ActPurchase.Services.Contracts.DTO;
using FluentValidation;

namespace ActPurchase.Services.Validators.Base
{
    /// <summary>
    ///  Базовый валидатор моделей актов
    /// </summary>
    public abstract class BaseActValidator<T> : AbstractValidator<T> where T : ActCreateModel
    {
        /// <summary>
        ///  Инициализирует валидатор и настраивает общие правила для акта
        /// </summary>
        protected BaseActValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty()
                .WithMessage("Номер обязателен")
                .MaximumLength(50)
                .WithMessage("Номер не может быть больше 50");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Город обязателен")
                .MaximumLength(30)
                .WithMessage("Город не может быть более 30 символов");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Тип обязателен")
                .MaximumLength(15)
                .WithMessage("Тип не может быть более 15 символов");
        }
    }
}
