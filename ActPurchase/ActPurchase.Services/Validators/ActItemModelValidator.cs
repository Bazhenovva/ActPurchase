using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Validators.Base;
using FluentValidation;

namespace ActPurchase.Services.Validators;

/// <summary>
/// Валидатор модели позиции акта
/// </summary>
public class ActItemModelValidator : BaseActItemValidator<ActItemModel>
{
    /// <summary>
    /// Инициализирует валидатор и настраивает правила
    /// </summary>
    public ActItemModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Идентификатор обязателен");

        RuleFor(x => x.Sum)
            .GreaterThan(0)
            .WithMessage("Сумма должна быть больше 0");
    }
}
