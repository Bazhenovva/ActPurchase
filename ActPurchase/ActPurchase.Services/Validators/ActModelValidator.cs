using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Validators.Base;
using FluentValidation;

namespace ActPurchase.Services.Validators;

/// <summary>
/// Валидатор модели акта
/// </summary>
public class ActModelValidator : BaseActValidator<ActModel>
{
    /// <summary>
    /// Инициализирует валидатор и настраивает правила
    /// </summary>
    public ActModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Идентификатор обязателен");

        RuleFor(x => x.TotalQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Итоговое количество не может быть отрицательным");

        RuleFor(x => x.TotalSum)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Итоговая сумма не может быть отрицательной");
    }
}
