using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Validators.Base;

namespace ActPurchase.Services.Validators.Counterparty;

/// <summary>
/// Валидатор модели создания контрагента
/// </summary>
public class CounterpartyCreateModelValidator : BaseCounterpartyValidator<CounterpartyCreateModel>;
