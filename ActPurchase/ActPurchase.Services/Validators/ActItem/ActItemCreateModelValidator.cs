using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Validators.Base;

namespace ActPurchase.Services.Validators.ActItem;

/// <summary>
/// Валидатор модели создания позиции акта
/// </summary>
public class ActItemCreateModelValidator : BaseActItemValidator<ActItemCreateModel>;
