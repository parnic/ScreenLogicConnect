using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace ScreenLogicConnect;

public class UnitConnection : IDisposable
{
    TcpClient? client;

    public async Task<bool> ConnectTo(EasyTouchUnit unit, string? password = null)
    {
        if (unit?.IPAddress == null)
        {
            return false;
        }

        if (client != null)
        {
            client.Dispose();
        }

        client = new TcpClient();
        await client.ConnectAsync(unit.IPAddress, unit.Port);

        var stream = client.GetStream();
        stream.Write(ConnectionMessageBytes);

        Debug.WriteLine("sending challenge string");
        stream.SendHLMessage(new Messages.ChallengeString());

        var recvBuf = new byte[8];
        var readBytes = await stream.ReadAsync(recvBuf);
        Debug.WriteLine("read {0} bytes (header)", readBytes);

        var recvBody = new byte[Messages.HLMessage.ExtractDataSize(recvBuf)];
        if (recvBody.Length == 0)
        {
            return false;
        }

        readBytes = await stream.ReadAsync(recvBody);
        Debug.WriteLine("read {0} bytes (body)", readBytes);
        string challengeStr = Messages.HLMessageTypeHelper.ExtractString(recvBody);

        Debug.WriteLine("sending login message");
        stream.SendHLMessage(CreateLoginMessage(new HLEncoder(password).GetEncryptedPassword(challengeStr)));

        readBytes = await stream.ReadAsync(recvBuf);
        Debug.WriteLine("read {0}", readBytes);
        recvBody = new byte[Messages.HLMessage.ExtractDataSize(recvBuf)];
        if (recvBody.Length > 0)
        {
            await stream.ReadAsync(recvBody);
        }
        return recvBuf[2] == Messages.ClientLogin.LoginAnswerId;
    }

    public async Task<Messages.GetPoolStatus?> GetPoolStatus(short senderId = 0) => await GetResponse<Messages.GetPoolStatus>(senderId);
    public async Task<Messages.GetControllerConfig?> GetControllerConfig(short senderId = 0) => await GetResponse<Messages.GetControllerConfig>(senderId);
    public async Task<Messages.GetMode?> GetMode(short senderId = 0) => await GetResponse<Messages.GetMode>(senderId);

    private async Task<T?> GetResponse<T>(short senderId) where T : Messages.HLMessage, new()
    {
        if (client == null)
        {
            return default;
        }

        Debug.WriteLine("sending status message");
        T msg = new();
        msg.Encode(senderId);
        client.GetStream().SendHLMessage(msg);
        return await BuildMessageFromStream<T>(client.GetStream());
    }

    internal static async Task<T?> BuildMessageFromStream<T>(NetworkStream ns) where T : Messages.HLMessage, new()
    {
        int bytesRead = 0;
        byte[] headerBuffer = new byte[8];
        while (bytesRead < 8)
        {
            try
            {
                bytesRead += await ns.ReadAsync(headerBuffer.AsMemory(bytesRead, 8 - bytesRead));
                if (bytesRead < 0)
                {
                    return null;
                }
            }
            catch { }
        }

        int msgDataSize = Messages.HLMessage.ExtractDataSize(headerBuffer);
        if (msgDataSize <= 0 || msgDataSize >= 100_000)
        {
            return null;
        }

        byte[] dataBuffer = new byte[msgDataSize];
        bytesRead = 0;
        while (bytesRead < msgDataSize)
        {
            bytesRead += await ns.ReadAsync(dataBuffer.AsMemory(bytesRead, msgDataSize - bytesRead));

            if (bytesRead < 0)
            {
                return null;
            }
        }

        Debug.WriteLine($"read {headerBuffer.Length + dataBuffer.Length} bytes of message data ({headerBuffer.Length} header, {dataBuffer.Length} data)");

        T msg = new();
        msg.ParseData(headerBuffer, dataBuffer);
        return msg;
    }

    private const string connectionMessage = "CONNECTSERVERHOST\r\n\r\n";
    private static ReadOnlySpan<byte> ConnectionMessageBytes => Encoding.ASCII.GetBytes(connectionMessage).AsSpan();

    private static Messages.HLMessage CreateLoginMessage(ReadOnlySpan<byte> encodedPwd)
    {
        Messages.ClientLogin login = new(0);
        login.Schema = 348;
        login.ConnectionType = 0;
        login.Version = "ScreenLogicConnect library";
        login.ProcID = 2;

        if (encodedPwd != null)
        {
            if (encodedPwd.Length > 16)
            {
                login.Password = new byte[16];
                encodedPwd[..16].CopyTo(login.Password);
            }
            else
            {
                login.Password = new byte[encodedPwd.Length];
                encodedPwd.CopyTo(login.Password);
            }
        }
        else
        {
            login.Password = new byte[16];
        }

        return login;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        client?.Dispose();
    }
}
