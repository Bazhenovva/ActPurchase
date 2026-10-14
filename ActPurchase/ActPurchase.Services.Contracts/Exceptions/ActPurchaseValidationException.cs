using ActPurchase.Common;

namespace ActPurchase.Services.Contracts.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при ошибках валидации модели
/// </summary>
public class ActPurchaseValidationException : ActPurchaseException
{
    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; }

    /// <summary>
    /// Создаёт исключение со списком ошибок валидации
    /// </summary>
    public ActPurchaseValidationException(IEnumerable<InvalidateItemModel> errors)
    {
        Errors = errors;
    }
}
