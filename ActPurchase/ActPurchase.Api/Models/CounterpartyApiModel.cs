namespace ActPurchase.Api.Models;

/// <summary>
/// DTO для чтения контрагента. Это то, что сервер отдаёт клиенту
/// </summary>
public class CounterpartyApiModel : CounterpartyCreateApiModel
{
    /// <summary>
    /// Идентификатор контрагента
    /// </summary>
    public Guid Id { get; set; }
}
