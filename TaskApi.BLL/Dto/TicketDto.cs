using Microsoft.AspNetCore.Http;
using TaskApi.BLL.Enum;

namespace TaskApi.BLL.Dto
{
    public class TicketDto
    {
        /// <summary>
        /// TicketId
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ticket create date
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
        public ICollection<IFormFile>? Files { get; set; }

        /// <summary>
        /// Ticket file pathes
        /// </summary>
        public List<string> FilePathes { get; set; } = [];
    }
}
