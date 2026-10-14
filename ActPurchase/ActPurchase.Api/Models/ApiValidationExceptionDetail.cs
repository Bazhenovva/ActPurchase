using ActPurchase.Common;

namespace ActPurchase.Api.Models;

/// <summary>
/// Детали ошибки валидации
/// </summary>
public class ApiValidationExceptionDetail
{
    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; set; } = Array.Empty<InvalidateItemModel>();
}
