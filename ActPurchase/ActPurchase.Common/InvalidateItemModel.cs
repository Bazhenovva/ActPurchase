namespace ActPurchase.Common;

/// <summary>
/// Модель элемента инвалидации
/// </summary>
public class InvalidateItemModel
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InvalidateItemModel"/>
    /// </summary>
    public InvalidateItemModel(string field, string message)
    {
        Field = field;
        Message = message;
    }

    /// <summary>
    /// Имя инвалидного поля
    /// </summary>
    public string Field { get; }

    /// <summary>
    /// Сообщение инвалидации
    /// </summary>
    public string Message { get; }
}
