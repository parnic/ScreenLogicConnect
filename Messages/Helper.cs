using System;
using System.Text;

namespace ScreenLogicConnect.Messages
{
    public class HLMessageTypeHelper
    {
        public static String extractString(sbyte[] data, ref int idx)
        {
            String result = "";
            int startIndex = idx;
            int bufferLength;
            int i;
            if (data[startIndex + 3] < (byte)0)
            {
                int i2 = startIndex + 3;
                data[i2] = (sbyte)(data[i2] + 128);
                bufferLength = ByteHelper.getIntFromByteArrayLittleEndian(data, idx);
                if (30000 <= bufferLength)
                {
                    return result;
                }
                startIndex += 4;
                for (i = startIndex; i < startIndex + bufferLength; i += 2)
                {
                    result = result + ((char)ByteHelper.getShortFromByteArrayAsLittleEndian(data, i));
                }
                idx = ((startIndex + bufferLength) + alignToNext4Boundary(bufferLength));
                return result;
            }
            bufferLength = ByteHelper.getIntFromByteArrayLittleEndian(data, idx);
            if (30000 <= bufferLength)
            {
                return result;
            }
            startIndex += 4;
            char[] temp = new char[bufferLength];
            for (i = startIndex; i < startIndex + bufferLength; i++)
            {
                temp[i - startIndex] = (char)(data[i] & 255);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(temp);
            result = sb.ToString();
            idx = ((startIndex + bufferLength) + alignToNext4Boundary(bufferLength));
            return result;
        }

        public static RgbColor extractColor(sbyte[] data, ref int idx)
        {
            int startIndex = idx;
            int r = ByteHelper.getIntFromByteArrayLittleEndian(data, startIndex);
            startIndex += 4;
            int g = ByteHelper.getIntFromByteArrayLittleEndian(data, startIndex);
            startIndex += 4;
            int b = ByteHelper.getIntFromByteArrayLittleEndian(data, startIndex);
            idx = startIndex + 4;
            return new RgbColor((byte)(r & 0xff), (byte)(g & 0xff), (byte)(b & 0xff));
        }

        private static int alignToNext4Boundary(int val)
        {
            int sub = val % 4;
            return sub == 0 ? 0 : 4 - sub;
        }
    }
}
