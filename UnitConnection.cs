using System;
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
            Console.WriteLine("sent challenge string");
            var recvBuf = new byte[1024];
            var readBytes = stream.Read(recvBuf, 0, recvBuf.Length);
            Console.WriteLine("read {0}", readBytes);

            stream.SendHLMessage(createLoginMessage(new sbyte[16]));
            Console.WriteLine("sent login message");
            readBytes = stream.Read(recvBuf, 0, recvBuf.Length);
            Console.WriteLine("read {0}", readBytes);
        }

        public Messages.GetPoolStatus GetPoolStatus()
        {
            client.GetStream().SendHLMessage(Messages.GetPoolStatus.QUERY(0));
            Console.WriteLine("sent status message");
            return new Messages.GetPoolStatus(getMessage(client.GetStream()));
        }

        private static Messages.HLMessage getMessage(NetworkStream ns)
        {
            int bytesRead = 0;
            sbyte[] headerBuffer = new sbyte[8];
            while (bytesRead < 8)
            {
                try
                {
                    bytesRead += ns.Read((byte[])(Array)headerBuffer, bytesRead, 8 - bytesRead);
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
            sbyte[] dataBuffer = new sbyte[msgDataSize];
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
            bytes[iLen + 0] = (byte)13;
            bytes[iLen + 1] = (byte)10;
            bytes[iLen + 2] = (byte)13;
            bytes[iLen + 3] = (byte)10;
            return bytes;
        }

        private Messages.HLMessage createLoginMessage(sbyte[] encodedPwd)
        {
            Messages.ClientLogin login = new Messages.ClientLogin((short)0, (short)27);
            login.set_schema(348);
            login.set_connectionType(0);
            login.set_version("Android");
            if (encodedPwd.Length > 16)
            {
                sbyte[] temp = new sbyte[16];
                for (int i = 0; i < 16; i++)
                {
                    temp[i] = encodedPwd[i];
                }
                login.set_byteArray(temp);
            }
            else
            {
                login.set_byteArray(encodedPwd);
            }
            login.set_procID(2);
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
