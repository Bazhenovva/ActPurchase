using ActPurchase.Services.Contracts.DTO;
using FluentValidation;

namespace ActPurchase.Services.Validators.Base
{
    /// <summary>
    /// Базовый валидатор моделей директоров
    /// </summary>
    /// <typeparam name="T">Тип проверяемой модели</typeparam>
    public abstract class BaseDirectorValidator<T> : AbstractValidator<T> where T : DirectorCreateModel
    {
        /// <summary>
        /// Инициализирует валидатор и настраивает общие правила
        /// </summary>
        protected BaseDirectorValidator()
        {
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Фамилия обязательна")
                .MaximumLength(20)
                .WithMessage("Фамилия не более 20 символов");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Имя обязательно")
                .MaximumLength(20)
                .WithMessage("Имя не более 20 символов");

            RuleFor(x => x.MiddleName)
                .MaximumLength(20)
                .WithMessage("Отчество не более 20 символов");

            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage("Название компании обязательно")
                .MaximumLength(70)
                .WithMessage("Название компании не более 70 символов");
        }
    }
}
