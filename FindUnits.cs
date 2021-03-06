﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ScreenLogicConnect
{
    public class FindUnits
    {
        public static readonly byte[] broadcastData = new byte[]
        {
            1, 0, 0, 0,
            0, 0, 0, 0,
        };

        protected const short multicastPort = 1444;
        private readonly Socket searchSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public static async Task<List<EasyTouchUnit>> Find()
        {
            var units = new List<EasyTouchUnit>();

            using (var udpClient = new UdpClient(new IPEndPoint(GetMyIP(), 53112))
            {
                EnableBroadcast = true,
                MulticastLoopback = false,
            })
            {
                await udpClient.SendAsync(broadcastData, broadcastData.Length, new IPEndPoint(IPAddress.Broadcast, multicastPort));

                var buf = await udpClient.ReceiveAsync().TimeoutAfter(TimeSpan.FromSeconds(1));
                if (buf != null && buf.RemoteEndPoint != null)
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

        public static IPAddress GetMyIP()
        {
            IPAddress localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address;
            }

            return localIP;
        }
    }
}
