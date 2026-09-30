using ActPurchase.Services.Contracts.DTO;
using FluentValidation;

namespace ActPurchase.Services.Validators.Base
{
    /// <summary>
    /// Базовый валидатор моделей контрагентов
    /// </summary>
    public abstract class BaseCounterpartyValidator<T> : AbstractValidator<T> where T : CounterpartyCreateModel
    {
        /// <summary>
        /// Инициализирует валидатор и настраивает общие правила
        /// </summary>
        protected BaseCounterpartyValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage("Название компании обязательно")
                .MaximumLength(70)
                .WithMessage("Название не более 70 символов");

            RuleFor(x => x.Inn)
                .NotEmpty()
                .WithMessage("ИНН обязателен")
                .MaximumLength(12)
                .WithMessage("ИНН не более 12 символов");

            RuleFor(x => x.Kpp)
                .MaximumLength(9)
                .WithMessage("КПП не более 9 символов");

            RuleFor(x => x.Ogrn)
                .MaximumLength(15)
                .WithMessage("ОГРН не более 15 символов");

            RuleFor(x => x.CheckingAccount)
                .MaximumLength(20)
                .WithMessage("Расчётный счёт не более 20 символов");

            RuleFor(x => x.CorrespondentAccount)
                .MaximumLength(20)
                .WithMessage("Корреспондентский счёт не более 20 символов");

            RuleFor(x => x.Bik)
                .MaximumLength(9)
                .WithMessage("БИК не более 9 символов");

            RuleFor(x => x.BankName)
                .MaximumLength(30)
                .WithMessage("Название банка не более 30 символов");

            RuleFor(x => x.LegalAddress)
                .MaximumLength(100)
                .WithMessage("Юридический адрес не более 100 символов");
        }
    }
}
