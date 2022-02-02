using System;
using System.IO;

namespace ScreenLogicConnect.Messages
{
    public class HLMessage
    {
        public const int HEADER_SIZE = 8;

        protected byte[]? data;
        protected MemoryStream? dataByteStream;
        protected byte[] header = new byte[HEADER_SIZE];
        protected MemoryStream? headerByteStream;
        protected int startIndex = 0;

        public HLMessage(short sender, short id)
        {
            headerByteStream = new MemoryStream(header);
            using (var mw = new BinaryWriter(headerByteStream))
            {
                mw.Write(sender);
                mw.Write(id);
                while (headerByteStream.Position < headerByteStream.Length)
                {
                    mw.Write((byte)0xff);
                }
            }
        }

        public HLMessage(ReadOnlySpan<byte> headerArray, ReadOnlySpan<byte> dataArray)
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

        public HLMessage(HLMessage msg)
            : this(msg != null ? msg.header : null, msg != null ? msg.data : null)
        {
        }

        public virtual Span<byte> AsByteArray()
        {
            var dataLength = data?.Length ?? 0;
            var result = new byte[dataLength + header.Length];
            using (var ms = new MemoryStream(result))
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(header, 0, 4);
                    bw.Write(dataLength);
                    if (data != null && dataLength > 0)
                    {
                        bw.Write(data);
                    }
                }
            }

            return result;
        }

        public static int ExtractDataSize(ReadOnlySpan<byte> buf)
        {
            return BitConverter.ToInt32(buf.Slice(4));
        }

        public short GetMessageID()
        {
            return BitConverter.ToInt16(header, 2);
        }

        public short GetMessageSender()
        {
            return BitConverter.ToInt16(header, 0);
        }

        public string GetMessageIDAsString()
        {
            return GetMessageID().ToString();
        }

        protected virtual void Decode()
        {
        }
    }
}
