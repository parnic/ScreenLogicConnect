using System;
using System.Net.Sockets;

namespace ScreenLogicConnect
{
    static class ExtensionMethods
    {
        public static void SendHLMessage(this NetworkStream stream, Messages.HLMessage msg)
        {
            var arr = msg.asByteArray();
            stream.Write((byte[])(Array)arr, 0, arr.Length);
        }
    }
}
