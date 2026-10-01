namespace ActPurchase.Services.Contracts.Exceptions
{
    /// <summary>
    /// Исключение для бизнес-ошибок
    /// </summary>
    public class ActPurchaseInvalidOperationException(string message) : ActPurchaseException(message);
}
