using System;
using System.IO;

namespace ScreenLogicConnect.Messages
{
    public class GetControllerConfig : HLMessage
    {
        private const int PUM_CIRC_COUNT = 8;

        public BodyDataStructure[] bodyArray { get; private set; }
        public int colorCount { get; private set; }
        public int m_CircuitCount { get; private set; }
        public PentLightColor[] m_ColorArray { get; private set; }
        public byte m_ControllerData { get; private set; }
        public int m_ControllerID { get; private set; }
        public byte m_ControllerType { get; private set; }
        public byte m_DegC { get; private set; }
        public int m_EquipFlags { get; private set; }
        public byte m_HWType { get; private set; }
        public int m_InterfaceTabFlags { get; private set; }
        public byte[] m_MaxSetPoint { get; private set; } = new byte[2];
        public byte[] m_MinSetPoint { get; private set; } = new byte[2];
        public byte[] m_PumpCircArray { get; private set; } = new byte[PUM_CIRC_COUNT];
        public int m_ShowAlarms { get; private set; }
        public String m_genCircuitName { get; private set; }

        public const short HLM_POOL_GETCTLRCONFIGQ = 12532;

        public static GetControllerConfig QUERY(short senderID)
        {
            return new GetControllerConfig(senderID, HLM_POOL_GETCTLRCONFIGQ);
        }

        private GetControllerConfig(short senderID, short msgID)
            : base(senderID, msgID)
        {
        }

        public GetControllerConfig(byte[] header, byte[] data)
            : base(header, data)
        {
        }

        public GetControllerConfig(HLMessage msg)
            : base(msg)
        {
        }

        public override byte[] AsByteArray()
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
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    m_ControllerID = br.ReadInt32();
                    for (int i = 0; i < 2; i++)
                    {
                        m_MinSetPoint[i] = br.ReadByte();
                        m_MaxSetPoint[i] = br.ReadByte();
                    }
                    m_DegC = br.ReadByte();
                    m_ControllerType = br.ReadByte();
                    m_HWType = br.ReadByte();
                    m_ControllerData = br.ReadByte();
                    m_EquipFlags = br.ReadInt32();
                    m_genCircuitName = HLMessageTypeHelper.ExtractString(br);
                    m_CircuitCount = br.ReadInt32();
                    bodyArray = new BodyDataStructure[m_CircuitCount];
                    for (int i = 0; i < m_CircuitCount; i++)
                    {
                        bodyArray[i] = new BodyDataStructure()
                        {
                            m_circuitID = br.ReadInt32(),
                            m_name = HLMessageTypeHelper.ExtractString(br),
                            m_nameIndex = br.ReadByte(),
                            m_function = br.ReadByte(),
                            m_interface = br.ReadByte(),
                            m_flags = br.ReadByte(),
                            m_colorSet = br.ReadByte(),
                            m_colorPos = br.ReadByte(),
                            m_colorStagger = br.ReadByte(),
                            m_deviceID = br.ReadByte(),
                            m_dfaultRT = br.ReadInt16(),
                            m_Pad1 = br.ReadByte(),
                            m_Pad2 = br.ReadByte(),
                        };
                    }
                    colorCount = br.ReadInt32();
                    m_ColorArray = new PentLightColor[colorCount];
                    for (int i = 0; i < colorCount; i++)
                    {
                        m_ColorArray[i] = new PentLightColor(HLMessageTypeHelper.ExtractString(br), HLMessageTypeHelper.ExtractColor(br));
                    }
                    for (int i = 0; i < PUM_CIRC_COUNT; i++)
                    {
                        m_PumpCircArray[i] = br.ReadByte();
                    }
                    m_InterfaceTabFlags = br.ReadInt32();
                    m_ShowAlarms = br.ReadInt32();
                }
            }
        }
    }
}
