using TaskApi.DAL.Entities;

namespace TaskApi.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Ticket, int> Tickets { get; }

        IRepository<TicketFile, Guid> TicketFiles { get; }
        Task SaveAsync();
    }
}
