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
        CreateMap<Act, ActModel>().ReverseMap();
        CreateMap<ActCreateModel, Act>();

        CreateMap<ActItem, ActItemModel>().ReverseMap();
        CreateMap<ActItemCreateModel, ActItem>();

        CreateMap<Counterparty, CounterpartyModel>().ReverseMap();
        CreateMap<CounterpartyCreateModel, Counterparty>();

        CreateMap<Director, DirectorModel>().ReverseMap();
        CreateMap<DirectorCreateModel, Director>();
    }
}
