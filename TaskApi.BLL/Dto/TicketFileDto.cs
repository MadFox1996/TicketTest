using TaskApi.DAL.Entities;

namespace TaskApi.BLL.Dto
{
    public class TicketFileDto
    {
        /// <summary>
        /// Ticket file id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ticket id
        /// </summary>
        public int TicketId { get; set; }

        /// <summary>
        /// Ticket
        /// </summary>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File hash
        /// </summary>
        public string Hash { get; set; }
    }
}
