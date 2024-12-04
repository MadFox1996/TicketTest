using TaskApi.BLL.Dto;

namespace TaskApi.BLL.Interfaces
{
    /// <summary>
    /// Service for Ticket
    /// </summary>
    public interface ITicketService
    {
        Task<int> CreateTaskAsync(TicketDto taskDto);

        Task<TicketDto> GetTaskAsync(int id);

        Task<int> UpdateTaskAsync(TicketDto taskDto);

        Task<bool> DeleteTaskAsync(int id); 
        
        void Dispose();
    }
}
