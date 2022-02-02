using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ScreenLogicConnect;

public class EasyTouchUnit
{
    public string? GatewayName { get; private set; }
    public byte GatewaySubType { get; private set; }
    public byte GatewayType { get; private set; }
    public IPAddress? IPAddress { get; private set; }
    public bool IsValid { get; private set; }
    public short Port { get; private set; }

    /// <summary>
    /// Creates a new EasyTouchUnit if the message contains valid unit information and null otherwise.
    /// </summary>
    /// <param name="result">The message received from a connection</param>
    /// <returns>The EasyTouchUnit if the message is valid</returns>
    public static EasyTouchUnit? Create(UdpReceiveResult result)
    {
        EasyTouchUnit unit = new(result);
        if (unit.IsValid)
        {
            return unit;
        }

        return null;
    }

    /// <summary>
    /// Creates a new EasyTouchUnit if the message contains valid unit information and null otherwise.
    /// </summary>
    /// <param name="data">The message received from a remote connection request</param>
    /// <returns>The EasyTouchUnit if the message is valid</returns>
    public static EasyTouchUnit? Create(Messages.GetGatewayData data)
    {
        EasyTouchUnit unit = new(data);
        if (unit.IsValid)
        {
            return unit;
        }

        return null;
    }

    public EasyTouchUnit(UdpReceiveResult result)
    {
        try
        {
            IPAddress = result.RemoteEndPoint.Address;

            using var ms = new MemoryStream(result.Buffer);
            using var br = new BinaryReader(ms);
            var unitType = br.ReadInt32();
            if (unitType == 2)
            {
                br.ReadBytes(4);
                Port = br.ReadInt16();
                GatewayType = br.ReadByte();
                GatewaySubType = br.ReadByte();
                GatewayName = Encoding.ASCII.GetString(result.Buffer.Skip((int)ms.Position).TakeWhile(x => x != 0).ToArray());

                IsValid = true;
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.StackTrace);
        }
    }

    public EasyTouchUnit(Messages.GetGatewayData data)
    {
        try
        {
            GatewayName = data.GatewayName;
            IPAddress = IPAddress.Parse(data.IPAddr!);
            Port = data.Port;
            IsValid = data.GatewayFound && data.PortOpen;
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.StackTrace);
        }
    }
}
