namespace TaskApi.DAL.Entities
{
    /// <summary>
    /// File related to ticket
    /// </summary>
    public class TicketFile
    {
        /// <summary>
        /// TicketFile id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// TicketId
        /// </summary>
        public int TicketId { get; set; }

        /// <summary>
        /// Ticket
        /// </summary>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Filename
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File hash
        /// </summary>
        public string Hash { get; set; }
    }
}
