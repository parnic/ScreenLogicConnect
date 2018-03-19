using System;
using System.Net;
using System.Net.Sockets;

namespace ScreenLogicConnect
{
    public class EasyTouchUnit
    {
        public String gatewayName { get; private set; }
        public byte gatewaySubType { get; private set; }
        public byte gatewayType { get; private set; }
        public IPAddress ipAddress { get; private set; }
        public bool isValid { get; private set; }
        public short port { get; private set; }

        public EasyTouchUnit(UdpReceiveResult result)
        {
            if (ByteHelper.getIntFromByteArrayLittleEndian((sbyte[])(Array)result.Buffer, 0) == 2)
            {
                int i;
                byte[] temp = new byte[4];
                for (i = 4; i < 8; i++)
                {
                    temp[i - 4] = result.Buffer[i];
                }
                try
                {
                    this.ipAddress = result.RemoteEndPoint.Address;
                    port = ByteHelper.getShortFromByteArrayAsLittleEndian((sbyte[])(Array)result.Buffer, 8);
                    gatewayType = result.Buffer[10];
                    gatewaySubType = result.Buffer[11];
                    int nameDataSize = 28;
                    i = 0;
                    while (i < nameDataSize)
                    {
                        i++;
                        if (result.Buffer[i + 12] == 0)
                        {
                            nameDataSize = i;
                        }
                    }
                    char[] nameData = new char[nameDataSize];
                    for (i = 0; i < nameDataSize; i++)
                    {
                        nameData[i] = (char)result.Buffer[i + 12];
                    }
                    gatewayName = new String(nameData);
                    isValid = true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
