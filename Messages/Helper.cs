using System;
using System.IO;

namespace ScreenLogicConnect.Messages
{
    public class HLMessageTypeHelper
    {
        public static String extractString(BinaryReader br)
        {
            var len = br.ReadInt32();
            var str = new String(br.ReadChars(len));
            while (len % 4 != 0)
            {
                br.Read();
                len++;
            }

            return str;
        }

        public static RgbColor extractColor(BinaryReader br)
        {
            return new RgbColor(
                (byte)(br.ReadInt32() & 0xff),
                (byte)(br.ReadInt32() & 0xff),
                (byte)(br.ReadInt32() & 0xff)
                );
        }

        public static int alignToNext4Boundary(int val)
        {
            int sub = val % 4;
            return sub == 0 ? 0 : 4 - sub;
        }
    }
}
