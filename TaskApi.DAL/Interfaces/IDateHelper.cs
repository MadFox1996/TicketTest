namespace TaskApi.DAL.Interfaces
{
    public interface IDateHelper
    {
        DateTime TodayDate { get; }
        string Today { get; }

        string DateFormat { get; }
    }
}