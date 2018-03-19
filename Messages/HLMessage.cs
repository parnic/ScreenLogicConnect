using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenLogicConnect.Messages
{
    public class HLMessage
    {
        public const int HEADER_SIZE = 8;
        protected sbyte[] data;
        protected List<SByte> dataByteArray;
        protected sbyte[] header;
        protected int startIndex;

        public HLMessage(short sender, short id)
        {
            this.header = new sbyte[8];
            this.dataByteArray = new List<SByte>();
            this.startIndex = 0;
            this.header[0] = ByteHelper.getLowByte(sender);
            this.header[1] = ByteHelper.getHighByte(sender);
            this.header[2] = ByteHelper.getLowByte(id);
            this.header[3] = ByteHelper.getHighByte(id);
            this.header[4] = -1;
            this.header[5] = -1;
            this.header[6] = -1;
            this.header[7] = -1;
        }

        public HLMessage(sbyte[] headerArray, sbyte[] dataArray)
        {
            int i;
            this.header = new sbyte[8];
            this.dataByteArray = new List<SByte>();
            this.startIndex = 0;
            for (i = 0; i < 8; i++)
            {
                this.header[i] = headerArray[i];
            }
            this.data = new sbyte[dataArray.Length];
            for (i = 0; i < dataArray.Length; i++)
            {
                this.data[i] = dataArray[i];
            }
            decode();
        }

        public HLMessage(HLMessage msg)
        {
            int i;
            this.header = new sbyte[8];
            this.dataByteArray = new List<SByte>();
            this.startIndex = 0;
            sbyte[] temp = msg.asByteArray();
            for (i = 0; i < 8; i++)
            {
                this.header[i] = temp[i];
            }
            int dataLength = temp.Length - 8;
            this.data = new sbyte[dataLength];
            for (i = 0; i < dataLength; i++)
            {
                this.data[i] = temp[i + 8];
            }
            decode();
        }

        public virtual sbyte[] asByteArray()
        {
            int i;
            int dataLength = this.dataByteArray.Count;
            sbyte[] result = new sbyte[(dataLength + 8)];
            for (i = 0; i < 8; i++)
            {
                result[i] = this.header[i];
            }
            result[4] = ByteHelper.getLowWordLowByte(dataLength);
            result[5] = ByteHelper.getLowWordHighByte(dataLength);
            result[6] = ByteHelper.getHighWordLowByte(dataLength);
            result[7] = ByteHelper.getHighWordHighByte(dataLength);
            for (i = 0; i < dataLength; i++)
            {
                result[i + 8] = dataByteArray[i];
            }
            return result;
        }

        public static int extractDataSize(sbyte[] data)
        {
            return ByteHelper.getIntFromByteArrayLittleEndian(data, 4);
        }

        public short getMessageID()
        {
            return ByteHelper.getShortFromByteArrayAsLittleEndian(this.header, 2);
        }

        public short getMessageSender()
        {
            return ByteHelper.getShortFromByteArrayAsLittleEndian(this.header, 0);
        }

        public String getMessageIDasString()
        {
            return getMessageID().ToString();
        }

        protected virtual void decode()
        {
            dataByteArray.AddRange(data);
        }

        protected int putByte(int startIndex, sbyte val)
        {
            return ByteHelper.putByte(this.data, startIndex, val);
        }

        protected int putShort(int startIndex, short val)
        {
            return ByteHelper.putShort(this.data, startIndex, val);
        }

        protected int putInteger(int startIndex, int val)
        {
            return ByteHelper.putInteger(this.data, startIndex, val);
        }

        protected int putByteArray(int startIndex, sbyte[] val)
        {
            return ByteHelper.putBytes(this.data, ByteHelper.putInteger(this.data, startIndex, val.Length), val);
        }

        protected int putString(int startIndex, String val)
        {
            sbyte[] stringBuffer;
            int bufferSize;
            if (ByteHelper.isUnicode(val))
            {
                stringBuffer = ByteHelper.convertAsUnicode(val);
                bufferSize = stringBuffer.Length | int.MinValue;
            }
            else
            {
                stringBuffer = (sbyte[])(Array)ASCIIEncoding.ASCII.GetBytes(val);
                bufferSize = stringBuffer.Length;
            }
            startIndex = ByteHelper.putBytes(this.data, putInteger(startIndex, bufferSize), stringBuffer);
            int iPadding = bufferSize % 4;
            if (iPadding == 0)
            {
                return startIndex;
            }
            return ByteHelper.putBytes(this.data, startIndex, new sbyte[(4 - iPadding)]);
        }

        protected void putByte(sbyte val)
        {
            ByteHelper.putByte(this.dataByteArray, val);
        }

        protected void putShort(short val)
        {
            ByteHelper.putShort(this.dataByteArray, val);
        }

        protected void putInteger(int val)
        {
            ByteHelper.putInteger(this.dataByteArray, val);
        }

        protected void putByteArray(sbyte[] val)
        {
            ByteHelper.putInteger(this.dataByteArray, val.Length);
            ByteHelper.putBytes(this.dataByteArray, val);
        }

        protected void putString(String val)
        {
            sbyte[] stringBuffer;
            int bufferSize;
            if (ByteHelper.isUnicode(val))
            {
                stringBuffer = ByteHelper.convertAsUnicode(val);
                bufferSize = stringBuffer.Length | int.MinValue;
            }
            else
            {
                stringBuffer = (sbyte[])(Array)ASCIIEncoding.ASCII.GetBytes(val);
                bufferSize = stringBuffer.Length;
            }
            putInteger(bufferSize);
            ByteHelper.putBytes(this.dataByteArray, stringBuffer);
            int iPadding = bufferSize % 4;
            if (iPadding != 0)
            {
                ByteHelper.putBytes(this.dataByteArray, new sbyte[(4 - iPadding)]);
            }
        }

        protected void putTime(HLTime hlTime)
        {
            ByteHelper.putShort(this.dataByteArray, hlTime.year);
            ByteHelper.putShort(this.dataByteArray, hlTime.month);
            ByteHelper.putShort(this.dataByteArray, hlTime.dayOfWeek);
            ByteHelper.putShort(this.dataByteArray, hlTime.day);
            ByteHelper.putShort(this.dataByteArray, hlTime.hour);
            ByteHelper.putShort(this.dataByteArray, hlTime.minute);
            ByteHelper.putShort(this.dataByteArray, hlTime.second);
            ByteHelper.putShort(this.dataByteArray, hlTime.millisecond);
        }
    }
}
