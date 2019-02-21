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

        public async Task<bool> ConnectTo(EasyTouchUnit unit, string password = null)
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
            string challengeStr = null;
            using (var ms = new MemoryStream(recvBuf))
            {
                using (var br = new BinaryReader(ms))
                {
                    br.ReadBytes(8);
                    challengeStr = Messages.HLMessageTypeHelper.extractString(br);
                }
            }

            Debug.WriteLine("sending login message");
            stream.SendHLMessage(CreateLoginMessage(new HLEncoder(password).GetEncryptedPassword(challengeStr)));

            readBytes = stream.Read(recvBuf, 0, recvBuf.Length);
            Debug.WriteLine("read {0}", readBytes);
            return recvBuf[2] == Messages.ClientLogin.HLM_CLIENT_LOGIN + 1;
        }

        public async Task<Messages.GetPoolStatus> GetPoolStatus()
        {
            Debug.WriteLine("sending status message");
            client.GetStream().SendHLMessage(Messages.GetPoolStatus.QUERY(0));
            return new Messages.GetPoolStatus(await GetMessage(client.GetStream()));
        }

        public async Task<Messages.GetControllerConfig> GetControllerConfig()
        {
            Debug.WriteLine("sending controller config message");
            client.GetStream().SendHLMessage(Messages.GetControllerConfig.QUERY(0));
            return new Messages.GetControllerConfig(await GetMessage(client.GetStream()));
        }

        public async Task<Messages.GetMode> GetMode()
        {
            Debug.WriteLine("sending get-mode message");
            client.GetStream().SendHLMessage(Messages.GetMode.QUERY(0));
            return new Messages.GetMode(await GetMessage(client.GetStream()));
        }

        public static async Task<Messages.HLMessage> GetMessage(NetworkStream ns)
        {
            int bytesRead = 0;
            byte[] headerBuffer = new byte[8];
            while (bytesRead < 8)
            {
                try
                {
                    bytesRead += await ns.ReadAsync(headerBuffer, bytesRead, 8 - bytesRead);
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
                bytesRead += await ns.ReadAsync((byte[])(Array)dataBuffer, bytesRead, msgDataSize - bytesRead);

                if (bytesRead < 0)
                {
                    return null;
                }
            }

            Debug.WriteLine($"read {headerBuffer.Length + dataBuffer.Length} bytes of message data ({headerBuffer.Length} header, {dataBuffer.Length} data)");

            return new Messages.HLMessage(headerBuffer, dataBuffer);
        }

        private const string connectionMessage = "CONNECTSERVERHOST\r\n\r\n";
        private byte[] CreateConnectServerSoftMessage()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Encoding.ASCII.GetBytes(connectionMessage));
                }

                return ms.ToArray();
            }
        }

        private Messages.HLMessage CreateLoginMessage(byte[] encodedPwd)
        {
            Messages.ClientLogin login = Messages.ClientLogin.QUERY(0);
            login.m_schema = 348;
            login.m_connectionType = 0;
            login.m_version = "ScreenLogicConnect library";
            login.m_procID = 2;

            if (encodedPwd == null)
            {
                encodedPwd = new byte[16];
            }

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
