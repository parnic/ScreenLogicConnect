using System;
using System.IO;

namespace ScreenLogicConnect
{
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
                using (var ms = new MemoryStream(data))
                {
                    using (var br = new BinaryReader(ms))
                    {
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

        public string toString()
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
