using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Validators.Base;
using FluentValidation;

namespace ActPurchase.Services.Validators;

/// <summary>
/// Валидатор модели директора
/// </summary>
public class DirectorModelValidator : BaseDirectorValidator<DirectorModel>
{
    /// <summary>
    /// /// <summary>
    /// Инициализирует валидатор и настраивает правило
    /// </summary>
    /// </summary>
    public DirectorModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Идентификатор обязателен");
    }
}
