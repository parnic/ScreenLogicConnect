namespace ScreenLogicConnect;

public class HLTime
{
    public const int size = 16;
    public short day = 1;
    public short dayOfWeek = 1;
    public short hour = 1;
    public short millisecond = 1;
    public short minute = 1;
    public short month = 1;
    public short second = 1;
    public short year = 2000;

    public HLTime(byte[] data, int startIndex)
    {
        if (data.Length - startIndex >= size)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            year = br.ReadInt16();
            month = br.ReadInt16();
            dayOfWeek = br.ReadInt16();
            day = br.ReadInt16();
            hour = br.ReadInt16();
            minute = br.ReadInt16();
            second = br.ReadInt16();
            millisecond = br.ReadInt16();
        }
    }

    public HLTime(short inYear, short inMonth, short inDayOfWeek, short inDay, short inHour, short inMinute, short inSecond, short inMillisecond)
    {
        year = inYear;
        month = inMonth;
        dayOfWeek = inDayOfWeek;
        day = inDay;
        hour = inHour;
        minute = inMinute;
        second = inSecond;
        millisecond = inMillisecond;
    }

    public override string ToString()
    {
        return "" + month + "/" + day + "/" + year;
    }

    public DateTime ToDate()
    {
        return new DateTime(year: (year - 2000) + 100, month: month - 1, day: day, hour: hour, minute: minute, second: second);
    }

    public long ToMilliseconds()
    {
        return (long)new TimeSpan(ToDate().Ticks).TotalMilliseconds;
    }
}
