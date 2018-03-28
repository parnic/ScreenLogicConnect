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
        public String gatewayName { get; private set; }
        public byte gatewaySubType { get; private set; }
        public byte gatewayType { get; private set; }
        public IPAddress ipAddress { get; private set; }
        public bool isValid { get; private set; }
        public short port { get; private set; }

        public EasyTouchUnit(UdpReceiveResult result)
        {
            try
            {
                ipAddress = result.RemoteEndPoint.Address;

                using (var ms = new MemoryStream(result.Buffer))
                {
                    using (var br = new BinaryReader(ms))
                    {
                        var unitType = br.ReadInt32();
                        if (unitType == 2)
                        {
                            br.ReadBytes(4);
                            port = br.ReadInt16();
                            gatewayType = br.ReadByte();
                            gatewaySubType = br.ReadByte();
                            gatewayName = Encoding.ASCII.GetString(result.Buffer.Skip((int)ms.Position).TakeWhile(x => x != 0).ToArray());

                            isValid = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }
    }
}
