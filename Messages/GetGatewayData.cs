namespace ScreenLogicConnect.Messages;

public class GetGatewayData : HLMessage
{
    public string? GatewayName;
    public bool GatewayFound;
    public bool LicenseOK;
    public string? IPAddr;
    public short Port;
    public bool PortOpen;
    public bool RelayOn;

    internal override short QueryId => 18003;

    public GetGatewayData(short senderID = 0)
            : base(senderID)
    {
    }

    public GetGatewayData(HLMessage msg)
        : base(msg)
    {
    }

    public GetGatewayData()
    {
    }

    public override Span<byte> AsByteArray()
    {
        if (!string.IsNullOrEmpty(GatewayName))
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                bw.WritePrefixLength(GatewayName);
                bw.WritePrefixLength(GatewayName);
            }

            data = ms.ToArray();
        }

        return base.AsByteArray();
    }

    protected override void Decode()
    {
        if (data == null)
        {
            return;
        }

        using var ms = new MemoryStream(data);
        using var br = new BinaryReader(ms);

        GatewayFound = br.ReadBoolean();
        LicenseOK = br.ReadBoolean();
        IPAddr = HLMessageTypeHelper.ExtractString(br);
        Port = br.ReadInt16();
        PortOpen = br.ReadBoolean();
        RelayOn = br.ReadBoolean();
    }
}
