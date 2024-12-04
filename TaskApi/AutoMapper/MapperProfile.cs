using AutoMapper;
using TaskApi.BLL.Dto;
using TaskApi.Models;

namespace TaskApi.AutoMapper
{
    public class TaskApiMappingProfile : Profile
    {
        public TaskApiMappingProfile()
        {
            CreateMap<TicketPostRequest, TicketDto>();
            CreateMap<TicketDto, TicketGetResponse>();
            CreateMap<TicketPutRequest, TicketDto>();

            CreateMap<BLL.Enum.TicketStage, Enum.TicketStage>();
            CreateMap<Enum.TicketStage, BLL.Enum.TicketStage>();
        }
    }
}
