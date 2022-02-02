namespace ScreenLogicConnect.Messages;

public class GetPoolStatus : HLMessage
{
    public CircuitUpdateDataStructure[]? CircuitArray { get; private set; }
    public int AirTemp { get; private set; }
    public int Alarms { get; private set; }
    public int BodiesCount { get; private set; }
    public int CircuitCount { get; private set; }
    public byte CleanerDelay { get; private set; }
    public int[] CoolSetPoint { get; private set; } = new int[2];
    public int[] CurrentTemp { get; private set; } = new int[2];
    public byte FreezeMode { get; private set; }
    public int[] HeatMode { get; private set; } = new int[2];
    public int[] HeatStatus { get; private set; } = new int[2];
    public int ORP { get; private set; }
    public int ORPTank { get; private set; }
    public int Ok { get; private set; }
    public int PH { get; private set; }
    public int PHTank { get; private set; }
    public byte PoolDelay { get; private set; }
    public byte Remotes { get; private set; }
    public int SaltPPM { get; private set; }
    public int Saturation { get; private set; }
    public int[] SetPoint { get; private set; } = new int[2];
    public byte SpaDelay { get; private set; }

    internal override short QueryId => 12526;

    internal GetPoolStatus(short senderID = 0)
            : base(senderID)
    {
    }

    public GetPoolStatus()
    {
    }

    public override Span<byte> AsByteArray()
    {
        using (var ms = new MemoryStream())
        {
            using (var bw = new BinaryWriter(ms))
            {
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
        Ok = br.ReadInt32();
        FreezeMode = br.ReadByte();
        Remotes = br.ReadByte();
        PoolDelay = br.ReadByte();
        SpaDelay = br.ReadByte();
        CleanerDelay = br.ReadByte();
        br.ReadBytes(3);
        AirTemp = br.ReadInt32();
        BodiesCount = br.ReadInt32();
        if (BodiesCount > 2)
        {
            BodiesCount = 2; // todo: what? this is how the android app is handling this, but it seems weird.
        }
        for (int i = 0; i < BodiesCount; i++)
        {
            var bodyType = br.ReadInt32();
            if (bodyType < 0 || bodyType >= 2)
            {
                bodyType = 0;
            }
            CurrentTemp[bodyType] = br.ReadInt32();
            HeatStatus[bodyType] = br.ReadInt32();
            SetPoint[bodyType] = br.ReadInt32();
            CoolSetPoint[bodyType] = br.ReadInt32();
            HeatMode[bodyType] = br.ReadInt32();
        }
        CircuitCount = br.ReadInt32();
        CircuitArray = new CircuitUpdateDataStructure[CircuitCount];
        for (int i = 0; i < CircuitCount; i++)
        {
            CircuitArray[i] = new CircuitUpdateDataStructure()
            {
                id = br.ReadInt32(),
                state = br.ReadInt32(),
                colorSet = br.ReadByte(),
                colorPos = br.ReadByte(),
                colorStagger = br.ReadByte(),
                delay = br.ReadByte(),
            };
        }
        PH = br.ReadInt32();
        ORP = br.ReadInt32();
        Saturation = br.ReadInt32();
        SaltPPM = br.ReadInt32();
        PHTank = br.ReadInt32();
        ORPTank = br.ReadInt32();
        Alarms = br.ReadInt32();
    }

    public bool IsDeviceready()
    {
        return Ok == 1;
    }

    public bool IsDeviceSync()
    {
        return Ok == 2;
    }

    public bool IsDeviceServiceMode()
    {
        return Ok == 3;
    }

    public bool IsSpaActive()
    {
        return CircuitArray?.Any(x => x.id == CircuitUpdateDataStructure.SPA_CIRCUIT_ID && x.state == 1) ?? false;
    }

    public bool IsPoolActive()
    {
        return CircuitArray?.Any(x => x.id == CircuitUpdateDataStructure.POOL_CIRCUIT_ID && x.state == 1) ?? false;
    }
}
