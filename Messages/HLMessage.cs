using System;
using System.IO;

namespace ScreenLogicConnect.Messages
{
    public class HLMessage
    {
        public const int HEADER_SIZE = 8;

        protected byte[] data;
        protected MemoryStream dataByteStream;
        protected byte[] header = new byte[HEADER_SIZE];
        protected MemoryStream headerByteStream;
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

        public HLMessage(byte[] headerArray, byte[] dataArray)
        {
            headerByteStream = new MemoryStream(header);
            using (var bw = new BinaryWriter(headerByteStream))
            {
                bw.Write(headerArray);
            }

            this.data = new byte[dataArray.Length];
            dataByteStream = new MemoryStream(data);
            using (var bw = new BinaryWriter(dataByteStream))
            {
                bw.Write(dataArray);
            }

            Decode();
        }

        public HLMessage(HLMessage msg)
            : this(msg.header, msg.data)
        {
        }

        public virtual byte[] AsByteArray()
        {
            var dataLength = this.data?.Length ?? 0;
            byte[] result = new byte[dataLength + this.header.Length];
            using (var bw = new BinaryWriter(new MemoryStream(result)))
            {
                bw.Write(this.header, 0, 4);
                bw.Write(dataLength);
                if (dataLength > 0)
                {
                    bw.Write(this.data);
                }
            }

            return result;
        }

        public static int ExtractDataSize(byte[] data)
        {
            return BitConverter.ToInt32(data, 4);
        }

        public short GetMessageID()
        {
            return BitConverter.ToInt16(header, 2);
        }

        public short GetMessageSender()
        {
            return BitConverter.ToInt16(header, 0);
        }

        public string GetMessageIDasString()
        {
            return GetMessageID().ToString();
        }

        protected virtual void Decode()
        {
        }
    }
}
