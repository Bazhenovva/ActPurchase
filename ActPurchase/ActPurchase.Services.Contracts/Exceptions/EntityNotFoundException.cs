namespace ActPurchase.Services.Contracts.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при отсутствии запрашиваемой сущности
    /// </summary>
    public class EntityNotFoundException<T>(Guid id) : ActPurchaseException($"Сущность {typeof(T).Name} с Id {id} не найдена");
}
