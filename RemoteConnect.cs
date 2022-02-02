using System.Net.Sockets;
using System.Threading.Tasks;

namespace ScreenLogicConnect
{
    public class RemoteConnect
    {
        public const int ServerDispatcherPort = 500;
        public const string ServerDispatcherURL = "screenlogicserver.pentair.com";

        public static async Task<EasyTouchUnit?> GetGatewayInfo(string systemName)
        {
            using var client = new TcpClient();
            await client.ConnectAsync(ServerDispatcherURL, ServerDispatcherPort);
            var ns = client.GetStream();
            ns.SendHLMessage(Messages.GetGatewayData.QUERY(systemName));

            var msg = await UnitConnection.GetMessage(ns);
            if (msg == null)
            {
                return null;
            }

            return EasyTouchUnit.Create(new Messages.GetGatewayData(msg));
        }
    }
}
