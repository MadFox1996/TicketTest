using AutoMapper;
using TaskApi.BLL.Dto;
using TaskApi.DAL.Entities;

namespace TaskApi.BLL.Automapper
{
    public class BLLMapperProfile:Profile
    {
        public BLLMapperProfile()
        {
            CreateMap<TicketDto, Ticket>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketFileDto, TicketFile>();
            CreateMap<TicketFile, TicketFileDto>();

            CreateMap<DAL.Enum.TicketStage, BLL.Enum.TicketStage>();
            CreateMap<BLL.Enum.TicketStage, DAL.Enum.TicketStage>();
        }
    }
}
