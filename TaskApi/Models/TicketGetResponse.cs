using TaskApi.Enum;

namespace TaskApi.Models
{
    public class TicketGetResponse
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
        public IList<string> FilePathes { get; set; }
    }
}
