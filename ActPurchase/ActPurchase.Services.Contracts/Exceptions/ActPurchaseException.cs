namespace ActPurchase.Services.Contracts.Exceptions
{
    /// <summary>
    /// Базовое исключение приложения
    /// </summary>
    public class ActPurchaseException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения
        /// </summary>
        public ActPurchaseException()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр исключения с сообщением
        /// </summary>
        public ActPurchaseException(string message) : base(message)
        {
        }
    }
}
