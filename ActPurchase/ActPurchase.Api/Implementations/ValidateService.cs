using ActPurchase.Common;
using ActPurchase.Services.Contracts.Exceptions;
using FluentValidation;

namespace ActPurchase.Api.Implementations;

/// <summary>
/// Сервис валидации. Проверяет модели через FluentValidation
/// </summary>
public class ValidateService : IValidateService
{
    private readonly Dictionary<Type, IValidator> validators = new();

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ValidateService"/>
    /// </summary>
    public ValidateService(IEnumerable<IValidator> validators)
    {
        foreach (var validator in validators)
        {
            var validatorType = validator.GetType();
            var modelType = validatorType.BaseType?.GenericTypeArguments.FirstOrDefault();

            if (modelType == null)
            {
                throw new InvalidOperationException($"Некорректный тип валидатора: {validator}");
            }

            this.validators[modelType] = validator;
        }
    }

    /// <summary>
    /// Валидирует модель и бросает исключение при ошибках
    /// </summary>
    public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken) where TModel : class
    {
        var modelType = model.GetType();
        if (!validators.TryGetValue(modelType, out var validator))
        {
            throw new InvalidOperationException($"Валидатор для {modelType} не найден");
        }

        var context = new ValidationContext<TModel>(model);
        var validationResult = await validator.ValidateAsync(context, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => new InvalidateItemModel(x.PropertyName, x.ErrorMessage));
            throw new ActPurchaseValidationException(errors);
        }
    }
}
