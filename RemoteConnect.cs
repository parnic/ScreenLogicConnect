using System.Net.Sockets;

namespace ScreenLogicConnect;

public class RemoteConnect
{
    public const int ServerDispatcherPort = 500;
    public const string ServerDispatcherURL = "screenlogicserver.pentair.com";

    public static async Task<EasyTouchUnit?> GetGatewayInfo(string systemName, short senderId = 0)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(ServerDispatcherURL, ServerDispatcherPort);
        var ns = client.GetStream();
        ns.SendHLMessage(new Messages.GetGatewayData(senderId) { GatewayName = systemName });

        var msg = await UnitConnection.BuildMessageFromStream<Messages.GetGatewayData>(ns);
        if (msg == null)
        {
            return null;
        }

        return EasyTouchUnit.Create(new Messages.GetGatewayData(msg));
    }
}
