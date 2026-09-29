namespace ActPurchase.Services.Contracts.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при отсутствии запрашиваемой сущности
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр с сообщением
        /// </summary>
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
