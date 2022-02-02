namespace ScreenLogicConnect.Messages;

public class HLMessageTypeHelper
{
    public static string ExtractString(BinaryReader br)
    {
        var len = br.ReadInt32();
        var str = new string(br.ReadChars(len));
        while (len % 4 != 0)
        {
            br.Read();
            len++;
        }

        return str;
    }

    public static string ExtractString(ReadOnlySpan<byte> buf)
    {
        var len = BitConverter.ToInt32(buf);
        int startIdx = sizeof(int);
        Span<char> chars = stackalloc char[len];
        for (int chIdx = 0; chIdx < len; chIdx++)
        {
            chars[chIdx] = (char)buf[startIdx + chIdx];
        }

        return new string(chars);
    }

    public static RgbColor ExtractColor(BinaryReader br)
    {
        return new RgbColor(
            (byte)(br.ReadInt32() & 0xff),
            (byte)(br.ReadInt32() & 0xff),
            (byte)(br.ReadInt32() & 0xff)
            );
    }

    public static int AlignToNext4Boundary(int val)
    {
        int sub = val % 4;
        return sub == 0 ? 0 : 4 - sub;
    }
}
