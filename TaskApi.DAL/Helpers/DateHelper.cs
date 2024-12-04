using TaskApi.DAL.Interfaces;

namespace TaskApi.DAL.Helpers
{
    /// <summary>
    /// Util class to store date through request
    /// </summary>
    public class DateHelper : IDateHelper
    {
        private readonly DateTime _currentDate;
        public DateHelper()
        {
            _currentDate = DateTime.Now;
        }
        public string Today => _currentDate.ToString(DateFormat);

        public DateTime TodayDate => _currentDate;

        public string DateFormat => "ddMMyyyy";       
    }
}
