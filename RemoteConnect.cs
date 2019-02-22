using System.Net.Sockets;
using System.Threading.Tasks;

namespace ScreenLogicConnect
{
    public class RemoteConnect
    {
        public const int ServerDispatcherPort = 500;
        public const string ServerDispatcherURL = "screenlogicserver.pentair.com";

        public static async Task<EasyTouchUnit> GetGatewayInfo(string systemName)
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(ServerDispatcherURL, ServerDispatcherPort);
                var ns = client.GetStream();
                ns.SendHLMessage(Messages.GetGatewayData.QUERY(systemName));
                return new EasyTouchUnit(new Messages.GetGatewayData(await UnitConnection.GetMessage(ns)));
            }
        }
    }
}
