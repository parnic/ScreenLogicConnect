using System.Net;
using System.Net.Sockets;

namespace ScreenLogicConnect;

public class FindUnits
{
    public static readonly byte[] broadcastData = new byte[]
    {
            1, 0, 0, 0,
            0, 0, 0, 0,
    };

    protected const short multicastPort = 1444;

    public static async Task<List<EasyTouchUnit>> Find()
    {
        var units = new List<EasyTouchUnit>();

        var myIP = GetMyIP();
        if (myIP == null)
        {
            return units;
        }

        using (var udpClient = new UdpClient(new IPEndPoint(myIP, 53112))
        {
            EnableBroadcast = true,
            MulticastLoopback = false,
        })
        {
            await udpClient.SendAsync(broadcastData, broadcastData.Length, new IPEndPoint(IPAddress.Broadcast, multicastPort));

            var buf = await udpClient.ReceiveAsync().TimeoutAfter(TimeSpan.FromSeconds(1));
            if (buf.RemoteEndPoint != null)
            {
                var findServerResponse = new EasyTouchUnit(buf);
                if (findServerResponse.IsValid)
                {
                    units.Add(findServerResponse);
                }
            }
        }

        return units;
    }

    public static IPAddress? GetMyIP()
    {
        IPAddress? localIP;
        using (Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            var endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint?.Address;
        }

        return localIP;
    }
}
