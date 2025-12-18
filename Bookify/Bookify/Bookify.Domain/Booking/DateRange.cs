using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Booking
{
    public record DateRange 
    {
        private DateRange(DateOnly start, DateOnly end)
        {
            Start = start;
            End = end;
        }

        public DateOnly Start { get; private set; }
        public DateOnly End { get; private set; }
        public int LengthInDays => End.DayNumber - Start.DayNumber;

        private static Error InValid => new("DateRange.InValid", "invalid date range start can not exceed end date");

        public static Result<DateRange> Create(DateOnly start, DateOnly end)
        {
            if(start>end)
            {
                return Result.Failure<DateRange>(InValid) ;
            }
            return new DateRange(start, end);
        }
    }

}
