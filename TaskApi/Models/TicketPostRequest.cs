namespace TaskApi.Models
{
    public class TicketPostRequest
    {
        /// <summary>
        /// Ticket files
        /// </summary>
        public IList<IFormFile>? Files { get; set; }

        /// <summary>
        /// Ticket name
        /// </summary>
        public string Name { get; set; }
    }
}
