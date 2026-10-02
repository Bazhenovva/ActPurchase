using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Validators.Base;

namespace ActPurchase.Services.Validators;

/// <summary>
/// Валидатор модели создания контрагента
/// </summary>
public class CounterpartyCreateModelValidator : BaseCounterpartyValidator<CounterpartyCreateModel>;
