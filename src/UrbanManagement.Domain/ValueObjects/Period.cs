namespace UrbanManagement.Domain.ValueObjects;

public class Period
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }


    public Period(DateTime start, DateTime end)
    {
        if (end < start)
            throw new ArgumentException("End date cannot be earlier than start date.");

            Start = start;
            End = end;
    }

    public TimeSpan Duration => End - Start;

    public bool IsOngoing => End > DateTime.Now;
    public bool IsInFuture => Start > DateTime.Now;
    public bool IsInPast => End < DateTime.Now;
    public bool IsOverlapping(Period other)
    {
        return Start < other.End && End > other.Start;
    }

    public override bool Equals(object? obj)
    {
        return obj is Period period && Start == period.Start && End == period.End;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }

    public override string ToString()
    {
        return $"{Start:yyyy-MM-dd} to {End:yyyy-MM-dd}";
    }
}
