using ActPurchase.Domain.Entites;
using ActPurchase.Services.Contracts.DTO;
using AutoMapper;

namespace ActPurchase.Services.Automapper;

/// <summary>
/// Профиль маппинга между сущностями и моделями сервисов
/// </summary>
public class ServiceProfile : Profile
{
    /// <summary>
    /// Инициализирует профиль маппинга
    /// </summary>
    public ServiceProfile()
    {
        CreateMap<Act, ActModel>();
        CreateMap<ActModel, Act>();
        CreateMap<ActCreateModel, Act>();

        CreateMap<ActItem, ActItemModel>();
        CreateMap<ActItemModel, ActItem>();
        CreateMap<ActItemCreateModel, ActItem>();

        CreateMap<Counterparty, CounterpartyModel>();
        CreateMap<CounterpartyModel, Counterparty>();
        CreateMap<CounterpartyCreateModel, Counterparty>();

        CreateMap<Director, DirectorModel>();
        CreateMap<DirectorModel, Director>();
        CreateMap<DirectorCreateModel, Director>();
    }
}
