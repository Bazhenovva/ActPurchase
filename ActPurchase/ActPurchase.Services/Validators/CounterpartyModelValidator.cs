using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Validators.Base;
using FluentValidation;

namespace ActPurchase.Services.Validators;

/// <summary>
/// Валидатор модели контрагента
/// </summary>
public class CounterpartyModelValidator : BaseCounterpartyValidator<CounterpartyModel>
{
    /// <summary>
    /// Инициализирует валидатор и настраивает правило
    /// </summary>
    public CounterpartyModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Идентификатор обязателен");
    }
}
