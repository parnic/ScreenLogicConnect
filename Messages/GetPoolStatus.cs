using System.IO;
using System.Linq;

namespace ScreenLogicConnect.Messages
{
    public class GetPoolStatus : HLMessage
    {
        public CircuitUpdateDataStructure[] circuitArray { get; private set; }
        public int m_AirTemp { get; private set; }
        public int m_Alarms { get; private set; }
        public int m_BodiesCount { get; private set; }
        public int m_CircuitCount { get; private set; }
        public byte m_CleanerDelay { get; private set; }
        public int[] m_CoolSetPoint { get; private set; } = new int[2];
        public int[] m_CurrentTemp { get; private set; } = new int[2];
        public byte m_FreezeMode { get; private set; }
        public int[] m_HeatMode { get; private set; } = new int[2];
        public int[] m_HeatStatus { get; private set; } = new int[2];
        public int m_ORP { get; private set; }
        public int m_ORPTank { get; private set; }
        public int m_Ok { get; private set; }
        public int m_PH { get; private set; }
        public int m_PHTank { get; private set; }
        public byte m_PoolDelay { get; private set; }
        public byte m_Remotes { get; private set; }
        public int m_SaltPPM { get; private set; }
        public int m_Saturation { get; private set; }
        public int[] m_SetPoint { get; private set; } = new int[2];
        public byte m_SpaDelay { get; private set; }

        public const short HLM_POOL_GETSTATUSQ = 12526;

        public static GetPoolStatus QUERY(short senderID)
        {
            return new GetPoolStatus(senderID, HLM_POOL_GETSTATUSQ);
        }

        private GetPoolStatus(short senderID, short msgID)
                : base(senderID, msgID)
        {
        }

        public GetPoolStatus(byte[] header, byte[] data)
                : base(header, data)
        {
        }

        public GetPoolStatus(HLMessage msg)
                : base(msg)
        {
        }

        public override byte[] asByteArray()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(0);
                }

                data = ms.ToArray();
            }

            return base.asByteArray();
        }

        protected override void decode()
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    m_Ok = br.ReadInt32();
                    m_FreezeMode = br.ReadByte();
                    m_Remotes = br.ReadByte();
                    m_PoolDelay = br.ReadByte();
                    m_SpaDelay = br.ReadByte();
                    m_CleanerDelay = br.ReadByte();
                    br.ReadBytes(3);
                    m_AirTemp = br.ReadInt32();
                    m_BodiesCount = br.ReadInt32();
                    if (m_BodiesCount > 2)
                    {
                        m_BodiesCount = 2; // todo: what? this is how the android app is handling this, but it seems weird.
                    }
                    for (int i = 0; i < m_BodiesCount; i++)
                    {
                        var bodyType = br.ReadInt32();
                        if (bodyType < 0 || bodyType >= 2)
                        {
                            bodyType = 0;
                        }
                        m_CurrentTemp[bodyType] = br.ReadInt32();
                        m_HeatStatus[bodyType] = br.ReadInt32();
                        m_SetPoint[bodyType] = br.ReadInt32();
                        m_CoolSetPoint[bodyType] = br.ReadInt32();
                        m_HeatMode[bodyType] = br.ReadInt32();
                    }
                    m_CircuitCount = br.ReadInt32();
                    circuitArray = new CircuitUpdateDataStructure[m_CircuitCount];
                    for (int i = 0; i < m_CircuitCount; i++)
                    {
                        circuitArray[i] = new CircuitUpdateDataStructure()
                        {
                            id = br.ReadInt32(),
                            state = br.ReadInt32(),
                            colorSet = br.ReadByte(),
                            colorPos = br.ReadByte(),
                            colorStagger = br.ReadByte(),
                            delay = br.ReadByte(),
                        };
                    }
                    m_PH = br.ReadInt32();
                    m_ORP = br.ReadInt32();
                    m_Saturation = br.ReadInt32();
                    m_SaltPPM = br.ReadInt32();
                    m_PHTank = br.ReadInt32();
                    m_ORPTank = br.ReadInt32();
                    m_Alarms = br.ReadInt32();
                }
            }
        }

        public bool isDeviceready()
        {
            return this.m_Ok == 1;
        }

        public bool isDeviceSync()
        {
            return this.m_Ok == 2;
        }

        public bool isDeviceServiceMode()
        {
            return this.m_Ok == 3;
        }

        public bool isSpaActive()
        {
            return this.circuitArray != null && this.circuitArray.Any(x => x.id == CircuitUpdateDataStructure.SPA_CIRCUIT_ID && x.state == 1);
        }

        public bool isPoolActive()
        {
            return this.circuitArray != null && this.circuitArray.Any(x => x.id == CircuitUpdateDataStructure.POOL_CIRCUIT_ID && x.state == 1);
        }
    }
}
