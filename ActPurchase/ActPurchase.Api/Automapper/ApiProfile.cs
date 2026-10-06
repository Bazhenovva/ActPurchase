using ActPurchase.Api.Models;
using ActPurchase.Services.Contracts.DTO;
using AutoMapper;

namespace ActPurchase.Api.Automapper
{
    /// <summary>
    /// маппинг дто в апи модель
    /// Внедряем снова модели, чтобы не путать слои и не использовать дто из сервисов напрямую
    /// </summary>
    public class ApiProfile : Profile
    {
        /// <summary>
        /// Настраивает маппинги
        /// </summary>
        public ApiProfile()
        {
            CreateMap<ActModel, ActApiModel>();
            CreateMap<ActApiModel, ActModel>();
            CreateMap<ActCreateApiModel, ActCreateModel>();
            CreateMap<ActCreateApiModel, ActModel>();

            CreateMap<ActItemModel, ActItemApiModel>();
            CreateMap<ActItemApiModel, ActItemModel>();
            CreateMap<ActItemCreateApiModel, ActItemCreateModel>();
            CreateMap<ActItemCreateApiModel, ActItemModel>();

            CreateMap<CounterpartyModel, CounterpartyApiModel>();
            CreateMap<CounterpartyApiModel, CounterpartyModel>();
            CreateMap<CounterpartyCreateApiModel, CounterpartyCreateModel>();
            CreateMap<CounterpartyCreateApiModel, CounterpartyModel>();

            CreateMap<DirectorModel, DirectorApiModel>();
            CreateMap<DirectorApiModel, DirectorModel>();
            CreateMap<DirectorCreateApiModel, DirectorCreateModel>();
            CreateMap<DirectorCreateApiModel, DirectorModel>();
        }
    }
}
