namespace ActPurchase.Services.Contracts.DTO;

/// <summary>
/// DTO для создания директора. Это то, что клиент присылает серверу
/// </summary>
public class DirectorCreateModel
{
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Отчество
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Название организации
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;
}
