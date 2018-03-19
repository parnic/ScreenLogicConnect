using System;

namespace ScreenLogicConnect
{
    public class HLTime
    {
        public const int size = 16;
        public short day = (short)1;
        public short dayOfWeek = (short)1;
        public short hour = (short)1;
        public short millisecond = (short)1;
        public short minute = (short)1;
        public short month = (short)1;
        public short second = (short)1;
        public short year = (short)2000;

        public HLTime(sbyte[] data, int startIndex)
        {
            if (data.Length - startIndex >= 16)
            {
                this.year = ByteHelper.getShortFromByteArrayAsLittleEndian(data, startIndex + 0);
                this.month = ByteHelper.getShortFromByteArrayAsLittleEndian(data, startIndex + 2);
                this.dayOfWeek = ByteHelper.getShortFromByteArrayAsLittleEndian(data, startIndex + 4);
                this.day = ByteHelper.getShortFromByteArrayAsLittleEndian(data, startIndex + 6);
                this.hour = ByteHelper.getShortFromByteArrayAsLittleEndian(data, startIndex + 8);
                this.minute = ByteHelper.getShortFromByteArrayAsLittleEndian(data, startIndex + 10);
                this.second = ByteHelper.getShortFromByteArrayAsLittleEndian(data, startIndex + 12);
                this.millisecond = ByteHelper.getShortFromByteArrayAsLittleEndian(data, startIndex + 14);
            }
        }

        public HLTime(short year, short month, short dayOfWeek, short day, short hour, short minute, short second, short millisecond)
        {
            this.year = year;
            this.month = month;
            this.dayOfWeek = dayOfWeek;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.millisecond = millisecond;
        }

        public String toString()
        {
            return "" + this.month + "/" + this.day + "/" + this.year;
        }

        public DateTime toDate()
        {
            return new DateTime(year: (this.year - 2000) + 100, month: month - 1, day: day, hour: hour, minute: minute, second: second);
        }

        public long toMilliseconds()
        {
            return (long)new TimeSpan(toDate().Ticks).TotalMilliseconds;
        }
    }
}
