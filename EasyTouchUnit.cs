using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ScreenLogicConnect
{
    public class EasyTouchUnit
    {
        public string GatewayName { get; private set; }
        public byte GatewaySubType { get; private set; }
        public byte GatewayType { get; private set; }
        public IPAddress IPAddress { get; private set; }
        public bool IsValid { get; private set; }
        public short Port { get; private set; }

        public EasyTouchUnit(UdpReceiveResult result)
        {
            try
            {
                IPAddress = result.RemoteEndPoint.Address;

                using (var ms = new MemoryStream(result.Buffer))
                {
                    using (var br = new BinaryReader(ms))
                    {
                        var unitType = br.ReadInt32();
                        if (unitType == 2)
                        {
                            br.ReadBytes(4);
                            Port = br.ReadInt16();
                            GatewayType = br.ReadByte();
                            GatewaySubType = br.ReadByte();
                            GatewayName = Encoding.ASCII.GetString(result.Buffer.Skip((int)ms.Position).TakeWhile(x => x != 0).ToArray());

                            IsValid = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        public EasyTouchUnit(Messages.GetGatewayData data)
        {
            try
            {
                GatewayName = data.GatewayName;
                IPAddress = IPAddress.Parse(data.IPAddr);
                Port = data.Port;
                IsValid = data.GatewayFound && data.PortOpen;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }
    }
}
