using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScreenLogicConnect
{
    public class ByteHelper
    {
        public static sbyte getLowWordLowByte(int v)
        {
            return (sbyte)(v & 255);
        }

        public static sbyte getLowWordHighByte(int v)
        {
            return (sbyte)((v >> 8) & 255);
        }

        public static sbyte getHighWordLowByte(int v)
        {
            return (sbyte)((v >> 16) & 255);
        }

        public static sbyte getHighWordHighByte(int v)
        {
            return (sbyte)((v >> 24) & 255);
        }

        public static int getIntFromByteArrayLittleEndian(sbyte[] data, int startIndex)
        {
            if (data.Length >= startIndex + 4)
            {
                return ((((((0 + (data[startIndex + 3] & 255)) << 8) + (data[startIndex + 2] & 255)) << 8) + (data[startIndex + 1] & 255)) << 8) + (data[startIndex + 0] & 255);
            }
            return 0;
        }

        public static short getShortFromByteArrayAsLittleEndian(sbyte[] data, int startIndex)
        {
            if (data.Length < startIndex + 4)
            {
                return (short)0;
            }
            return (short)((data[startIndex + 0] & 255) + ((short)(((short)((data[startIndex + 1] & 255) + 0)) << 8)));
        }

        public static short getShortFromByteArrayAsBigEndian(sbyte[] data, int startIndex)
        {
            if (data.Length < startIndex + 4)
            {
                return (short)0;
            }
            return (short)((data[startIndex + 1] & 255) + ((short)(((short)((data[startIndex + 0] & 255) + 0)) << 8)));
        }

        public static sbyte getUnsignedByteFromByteArray(sbyte[] data, int startIndex)
        {
            return (sbyte)(data[startIndex] & 255);
        }

        public static sbyte getHighByte(short v)
        {
            return (sbyte)(v >> 8);
        }

        public static sbyte getLowByte(short v)
        {
            return (sbyte)v;
        }

        public static sbyte[] getBigEndian(short v)
        {
            return new sbyte[] { getHighByte(v), getLowByte(v) };
        }

        public static sbyte[] getLittleEndian(short v)
        {
            return new sbyte[] { getLowByte(v), getHighByte(v) };
        }

        public static sbyte[] getBigEndian(int v)
        {
            return new sbyte[] { getHighWordHighByte(v), getHighWordLowByte(v), getLowWordHighByte(v), getLowWordLowByte(v) };
        }

        public static sbyte[] getLittleEndianInt(int v)
        {
            return new sbyte[] { getLowWordLowByte(v), getLowWordHighByte(v), getHighWordLowByte(v), getHighWordHighByte(v) };
        }

        public static sbyte[] getLittleEndianShort(short v)
        {
            return new sbyte[] { getLowWordLowByte(v), getLowWordHighByte(v) };
        }

        public static bool isUnicode(String s)
        {
            const int MaxAnsiCode = 255;
            return s.Any(c => c > MaxAnsiCode);
        }

        public static sbyte[] convertAsUnicode(String s)
        {
            if (isUnicode(s))
            {
                return (sbyte[])(Array)Encoding.Unicode.GetBytes(s);
            }
            else
            {
                return (sbyte[])(Array)Encoding.ASCII.GetBytes(s);
            }
        }

        public static int putInteger(sbyte[] data, int startIndex, int val)
        {
            sbyte[] temp = getLittleEndianInt(val);
            data[startIndex + 0] = temp[0];
            data[startIndex + 1] = temp[1];
            data[startIndex + 2] = temp[2];
            data[startIndex + 3] = temp[3];
            return startIndex + 4;
        }

        public static int putShort(sbyte[] data, int startIndex, short val)
        {
            sbyte[] temp = getLittleEndianShort(val);
            data[startIndex + 0] = temp[0];
            data[startIndex + 1] = temp[1];
            return startIndex + 2;
        }

        public static int putByte(sbyte[] data, int startIndex, sbyte val)
        {
            data[startIndex] = (sbyte)(val & 255);
            return startIndex + 1;
        }

        public static int putBytes(sbyte[] data, int startIndex, sbyte[] val)
        {
            foreach (sbyte b in val)
            {
                data[startIndex] = b;
                startIndex++;
            }
            return startIndex;
        }

        public static void putInteger(List<SByte> data, int val)
        {
            sbyte[] temp = getLittleEndianInt(val);
            data.Add(temp[0]);
            data.Add(temp[1]);
            data.Add(temp[2]);
            data.Add(temp[3]);
        }

        public static void putShort(List<SByte> data, short val)
        {
            sbyte[] temp = getLittleEndianShort(val);
            data.Add(temp[0]);
            data.Add(temp[1]);
        }

        public static void putByte(List<SByte> data, sbyte val)
        {
            data.Add((sbyte)(val & 255));
        }

        public static void putBytes(List<SByte> data, sbyte[] val)
        {
            foreach (sbyte valueOf in val)
            {
                data.Add(valueOf);
            }
        }
    }
}
