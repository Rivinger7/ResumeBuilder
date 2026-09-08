namespace ResumeBuilder.Domain.Helpers;

public static class CustomTimeProvider
{
    public static Func<DateTimeOffset> UtcNowOffsetProvider { get; set; } = () => DateTimeOffset.UtcNow;
    public static Func<DateTime> UtcNowProvider { get; set; } = () => DateTime.UtcNow;
    public static DateTimeOffset UtcNowOffset => UtcNowOffsetProvider();
    public static DateTime UtcNow => UtcNowProvider();

    public static DateTime GetUtcPlus7Time()
    {
        return DateTime.UtcNow.AddHours(7);
    }

    public static DateTimeOffset GetUtcPlus7TimeOffset()
    {
        return DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));
    }

    public static DateOnly GetUtcPlus7DateOnly()
    {
        DateTime utcPlus7Now = DateTime.UtcNow.AddHours(7);
        return DateOnly.FromDateTime(utcPlus7Now);
    }

    [Obsolete("Should not use this method if using DB instead use AddMinutesToUtcNow")]
    public static DateTimeOffset AddMinutesToUtcPlus7(int minutes)
    {
        return GetUtcPlus7TimeOffset().AddMinutes(minutes);
    }

    public static DateTimeOffset AddMinutesToUtcNowOffset(int minutes)
    {
        return UtcNowOffset.AddMinutes(minutes);
    }

    public static DateTime AddMinutesToUtcNow(int minutes)
    {
        return UtcNow.AddMinutes(minutes);
    }
}
