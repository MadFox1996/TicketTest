using TaskApi.Enum;

namespace TaskApi.Models
{
    public class TicketPutRequest
    {
        /// <summary>
        /// Ticket files
        /// </summary>
        public IList<IFormFile>? Files { get; set; }

        /// <summary>
        /// Ticket name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ticket processing stage
        /// </summary>
        public TicketStage Stage { get; set; }
    }
}
