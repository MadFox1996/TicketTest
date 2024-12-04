using TaskApi.DAL.Enum;

namespace TaskApi.DAL.Entities
{
    /// <summary>
    /// Ticket entity
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Ticket id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ticket created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Ticket name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ticket processing stage
        /// </summary>
        public TicketStage Stage { get; set; }

        /// <summary>
        /// Ticket files
        /// </summary>
        public ICollection<TicketFile> TicketFiles { get; set; }
    }
}
