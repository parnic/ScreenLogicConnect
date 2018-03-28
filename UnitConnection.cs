using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLogicConnect
{
    public class UnitConnection : IDisposable
    {
        TcpClient client;

        public async Task ConnectTo(EasyTouchUnit unit)
        {
            if (client != null)
            {
                client.Dispose();
            }

            client = new TcpClient();
            await client.ConnectAsync(unit.ipAddress, unit.port);

            var connMsg = CreateConnectServerSoftMessage();
            var stream = client.GetStream();
            stream.Write(connMsg, 0, connMsg.Length);

            Debug.WriteLine("sending challenge string");
            stream.SendHLMessage(Messages.ChallengeString.QUERY(0));

            var recvBuf = new byte[1024];
            var readBytes = stream.Read(recvBuf, 0, recvBuf.Length);
            Debug.WriteLine("read {0}", readBytes);

            Debug.WriteLine("sending login message");
            stream.SendHLMessage(createLoginMessage(new byte[16]));

            readBytes = stream.Read(recvBuf, 0, recvBuf.Length);
            Debug.WriteLine("read {0}", readBytes);
        }

        public Messages.GetPoolStatus GetPoolStatus()
        {
            Debug.WriteLine("sending status message");
            client.GetStream().SendHLMessage(Messages.GetPoolStatus.QUERY(0));
            return new Messages.GetPoolStatus(getMessage(client.GetStream()));
        }

        public Messages.GetControllerConfig GetControllerConfig()
        {
            Debug.WriteLine("sending controller config message");
            client.GetStream().SendHLMessage(Messages.GetControllerConfig.QUERY(0));
            return new Messages.GetControllerConfig(getMessage(client.GetStream()));
        }

        public Messages.GetMode GetMode()
        {
            Debug.WriteLine("sending get-mode message");
            client.GetStream().SendHLMessage(Messages.GetMode.QUERY(0));
            return new Messages.GetMode(getMessage(client.GetStream()));
        }

        private static Messages.HLMessage getMessage(NetworkStream ns)
        {
            int bytesRead = 0;
            byte[] headerBuffer = new byte[8];
            while (bytesRead < 8)
            {
                try
                {
                    bytesRead += ns.Read(headerBuffer, bytesRead, 8 - bytesRead);
                    if (bytesRead < 0)
                    {
                        return null;
                    }
                }
                catch { }
            }

            int msgDataSize = Messages.HLMessage.extractDataSize(headerBuffer);
            if (msgDataSize <= 0 || msgDataSize >= 100000)
            {
                return null;
            }

            byte[] dataBuffer = new byte[msgDataSize];
            bytesRead = 0;
            while (bytesRead < msgDataSize)
            {
                bytesRead += ns.Read((byte[])(Array)dataBuffer, bytesRead, msgDataSize - bytesRead);

                if (bytesRead < 0)
                {
                    return null;
                }
            }

            Debug.WriteLine($"read {headerBuffer.Length + dataBuffer.Length} bytes of message data ({headerBuffer.Length} header, {dataBuffer.Length} data)");

            return new Messages.HLMessage(headerBuffer, dataBuffer);
        }

        private static string connectionMessage = "CONNECTSERVERHOST";
        private byte[] CreateConnectServerSoftMessage()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Encoding.ASCII.GetBytes(connectionMessage));
                    bw.Write((byte)'\r');
                    bw.Write((byte)'\n');
                    bw.Write((byte)'\r');
                    bw.Write((byte)'\n');
                }

                return ms.ToArray();
            }
        }

        private Messages.HLMessage createLoginMessage(byte[] encodedPwd)
        {
            Messages.ClientLogin login = new Messages.ClientLogin((short)0, (short)27);
            login.m_schema = 348;
            login.m_connectionType = 0;
            login.m_version = "ScreenLogicConnect library";
            login.m_procID = 2;

            if (encodedPwd.Length > 16)
            {
                login.m_byteArray = new byte[16];
                Array.Copy(encodedPwd, login.m_byteArray, 16);
            }
            else
            {
                login.m_byteArray = encodedPwd;
            }

            return login;
        }

        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
            }
        }
    }
}
