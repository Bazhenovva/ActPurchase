namespace ActPurchase.Services.Contracts.DTO;

/// <summary>
/// DTO для чтения контрагента. Это то, что сервер отдаёт клиенту
/// </summary>
public class CounterpartyModel : CounterpartyCreateModel
{
    /// <summary>
    /// Идентификатор контрагента
    /// </summary>
    public Guid Id { get; set; }
}
