namespace ScreenLogicConnect.Messages;

public class GetControllerConfig : HLMessage
{
    private const int PUM_CIRC_COUNT = 8;

    public BodyDataStructure[]? BodyArray { get; private set; }
    public int ColorCount { get; private set; }
    public int CircuitCount { get; private set; }
    public PentLightColor[]? ColorArray { get; private set; }
    public byte ControllerData { get; private set; }
    public int ControllerID { get; private set; }
    public byte ControllerType { get; private set; }
    public byte DegC { get; private set; }
    public int EquipFlags { get; private set; }
    public byte HWType { get; private set; }
    public int InterfaceTabFlags { get; private set; }
    public byte[] MaxSetPoint { get; private set; } = new byte[2];
    public byte[] MinSetPoint { get; private set; } = new byte[2];
    public byte[] PumpCircArray { get; private set; } = new byte[PUM_CIRC_COUNT];
    public int ShowAlarms { get; private set; }
    public string? GenCircuitName { get; private set; }

    internal override short QueryId => 12532;

    internal GetControllerConfig(short senderID = 0)
        : base(senderID)
    {
    }

    public GetControllerConfig()
    {
    }

    public override Span<byte> AsByteArray()
    {
        using (var ms = new MemoryStream())
        {
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(0);
                bw.Write(0);
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
        ControllerID = br.ReadInt32();
        for (int i = 0; i < 2; i++)
        {
            MinSetPoint[i] = br.ReadByte();
            MaxSetPoint[i] = br.ReadByte();
        }
        DegC = br.ReadByte();
        ControllerType = br.ReadByte();
        HWType = br.ReadByte();
        ControllerData = br.ReadByte();
        EquipFlags = br.ReadInt32();
        GenCircuitName = HLMessageTypeHelper.ExtractString(br);
        CircuitCount = br.ReadInt32();
        BodyArray = new BodyDataStructure[CircuitCount];
        for (int i = 0; i < CircuitCount; i++)
        {
            BodyArray[i] = new BodyDataStructure()
            {
                CircuitID = br.ReadInt32(),
                Name = HLMessageTypeHelper.ExtractString(br),
                NameIndex = br.ReadByte(),
                Function = br.ReadByte(),
                Interface = br.ReadByte(),
                Flags = br.ReadByte(),
                ColorSet = br.ReadByte(),
                ColorPos = br.ReadByte(),
                ColorStagger = br.ReadByte(),
                DeviceID = br.ReadByte(),
                DfaultRT = br.ReadInt16(),
                Pad1 = br.ReadByte(),
                Pad2 = br.ReadByte(),
            };
        }
        ColorCount = br.ReadInt32();
        ColorArray = new PentLightColor[ColorCount];
        for (int i = 0; i < ColorCount; i++)
        {
            ColorArray[i] = new PentLightColor(HLMessageTypeHelper.ExtractString(br), HLMessageTypeHelper.ExtractColor(br));
        }
        for (int i = 0; i < PUM_CIRC_COUNT; i++)
        {
            PumpCircArray[i] = br.ReadByte();
        }
        InterfaceTabFlags = br.ReadInt32();
        ShowAlarms = br.ReadInt32();
    }
}
