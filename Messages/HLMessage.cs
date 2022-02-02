namespace ScreenLogicConnect.Messages;

public abstract class HLMessage
{
    public const int HEADER_SIZE = 8;

    protected byte[]? data;
    protected MemoryStream? dataByteStream;
    protected byte[] header = new byte[HEADER_SIZE];
    protected MemoryStream? headerByteStream;
    protected int startIndex = 0;

    internal HLMessage()
    {

    }

    public HLMessage(short sender)
    {
        Encode(sender);
    }

    public HLMessage(ReadOnlySpan<byte> headerArray, ReadOnlySpan<byte> dataArray)
    {
        ParseData(headerArray, dataArray);
    }

    public HLMessage(HLMessage msg)
        : this(msg?.header, msg?.data)
    {
    }

    public void ParseData(ReadOnlySpan<byte> headerArray, ReadOnlySpan<byte> dataArray)
    {
        if (headerArray != null)
        {
            header = new byte[headerArray.Length];
            headerArray.CopyTo(header);
        }
        if (dataArray != null)
        {
            data = new byte[dataArray.Length];
            dataArray.CopyTo(data);
        }

        if (data != null)
        {
            Decode();
        }
    }

    internal abstract short QueryId { get; }
    internal short AnswerId => (short)(QueryId + 1);

    internal void Encode(short senderId = 0)
    {
        headerByteStream = new MemoryStream(header);
        using var mw = new BinaryWriter(headerByteStream);
        mw.Write(senderId);
        mw.Write(QueryId);
        while (headerByteStream.Position < headerByteStream.Length)
        {
            mw.Write((byte)0xff);
        }
    }

    public virtual Span<byte> AsByteArray()
    {
        var dataLength = data?.Length ?? 0;
        var result = new byte[dataLength + header.Length];
        using (var ms = new MemoryStream(result))
        {
            using var bw = new BinaryWriter(ms);
            bw.Write(header, 0, 4);
            bw.Write(dataLength);
            if (data != null && dataLength > 0)
            {
                bw.Write(data);
            }
        }

        return result;
    }

    public static int ExtractDataSize(ReadOnlySpan<byte> buf) => BitConverter.ToInt32(buf[4..]);

    public short MessageId => BitConverter.ToInt16(header, 2);

    public short SenderId => BitConverter.ToInt16(header, 0);

    protected virtual void Decode()
    {
    }
}
