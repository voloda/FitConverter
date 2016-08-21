using System;

namespace FitConverter
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}