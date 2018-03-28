using System;
using System.Diagnostics;
using System.Net.Sockets;
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
            stream.SendHLMessage(Messages.ChallengeString.QUERY(0));
            Debug.WriteLine("sent challenge string");
            var recvBuf = new byte[1024];
            var readBytes = stream.Read(recvBuf, 0, recvBuf.Length);
            Debug.WriteLine("read {0}", readBytes);

            stream.SendHLMessage(createLoginMessage(new byte[16]));
            Debug.WriteLine("sent login message");
            readBytes = stream.Read(recvBuf, 0, recvBuf.Length);
            Debug.WriteLine("read {0}", readBytes);
        }

        public Messages.GetPoolStatus GetPoolStatus()
        {
            client.GetStream().SendHLMessage(Messages.GetPoolStatus.QUERY(0));
            Debug.WriteLine("sent status message");
            return new Messages.GetPoolStatus(getMessage(client.GetStream()));
        }

        public Messages.GetControllerConfig GetControllerConfig()
        {
            client.GetStream().SendHLMessage(Messages.GetControllerConfig.QUERY(0));
            Debug.WriteLine("sent controller config message");
            return new Messages.GetControllerConfig(getMessage(client.GetStream()));
        }

        public Messages.GetMode GetMode()
        {
            client.GetStream().SendHLMessage(Messages.GetMode.QUERY(0));
            Debug.WriteLine("sent get-mode message");
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
            return new Messages.HLMessage(headerBuffer, dataBuffer);
        }

        private static string connectionMessage = "CONNECTSERVERHOST";
        private byte[] CreateConnectServerSoftMessage()
        {
            int iLen = connectionMessage.Length;
            byte[] bytes = new byte[(iLen + 4)];
            var connBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(connectionMessage);
            for (int i = 0; i < iLen; i++)
            {
                bytes[i] = connBytes[i];
            }
            bytes[iLen + 0] = (byte)'\r';
            bytes[iLen + 1] = (byte)'\n';
            bytes[iLen + 2] = (byte)'\r';
            bytes[iLen + 3] = (byte)'\n';
            return bytes;
        }

        private Messages.HLMessage createLoginMessage(byte[] encodedPwd)
        {
            Messages.ClientLogin login = new Messages.ClientLogin((short)0, (short)27);
            login.m_schema = 348;
            login.m_connectionType = 0;
            login.m_version = "ScreenLogicConnect library";
            if (encodedPwd.Length > 16)
            {
                byte[] temp = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    temp[i] = encodedPwd[i];
                }
                login.m_byteArray = temp;
            }
            else
            {
                login.m_byteArray = encodedPwd;
            }
            login.m_procID = 2;
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
