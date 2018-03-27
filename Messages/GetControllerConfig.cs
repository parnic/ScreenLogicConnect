using System;

namespace ScreenLogicConnect.Messages
{
    public class GetControllerConfig : HLMessage
    {
        private const int PUM_CIRC_COUNT = 8;
        private BodyDataStructure[] bodyArray;
        private int colorCount;
        private int m_CircuitCount;
        private PentLightColor[] m_ColorArray;
        private byte m_ControllerData;
        private int m_ControllerID;
        private byte m_ControllerType;
        private byte m_DegC;
        private int m_EquipFlags;
        private byte m_HWType;
        private int m_InterfaceTabFlags;
        private byte[] m_MaxSetPoint;
        private byte[] m_MinSetPoint;
        private byte[] m_PumpCircArray;
        private int m_ShowAlarms;
        private String m_genCircuitName;

        public const short HLM_POOL_GETCTLRCONFIGQ = (short)12532;

        public static GetControllerConfig QUERY(short senderID)
        {
            return new GetControllerConfig(senderID, (short)HLM_POOL_GETCTLRCONFIGQ);
        }

        private GetControllerConfig(short senderID, short msgID)
            : base(senderID, msgID)
        {
        }

        public GetControllerConfig(sbyte[] header, sbyte[] data)
            : base(header, data)
        {
        }

        public GetControllerConfig(HLMessage msg)
            : base(msg)
        {
        }

        public override sbyte[] asByteArray()
        {
            putInteger(0);
            putInteger(0);
            return base.asByteArray();
        }

        protected override void decode()
        {
            int i;
            this.m_MinSetPoint = new byte[2];
            this.m_MaxSetPoint = new byte[2];
            this.startIndex = 0;
            this.m_ControllerID = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_MinSetPoint[0] = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_MaxSetPoint[0] = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_MinSetPoint[1] = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_MaxSetPoint[1] = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_DegC = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_ControllerType = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_HWType = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_ControllerData = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_EquipFlags = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_genCircuitName = HLMessageTypeHelper.extractString(this.data, ref this.startIndex);
            this.m_CircuitCount = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.bodyArray = new BodyDataStructure[this.m_CircuitCount];
            for (i = 0; i < this.m_CircuitCount; i++)
            {
                this.bodyArray[i] = new BodyDataStructure();
                this.bodyArray[i].m_circuitID = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
                this.bodyArray[i].m_name = HLMessageTypeHelper.extractString(this.data, ref this.startIndex);
                this.bodyArray[i].m_nameIndex = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_function = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_interface = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_flags = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_colorSet = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_colorPos = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_colorStagger = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_deviceID = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_dfaultRT = ByteHelper.getShortFromByteArrayAsLittleEndian(this.data, this.startIndex);
                this.startIndex += 2;
                this.bodyArray[i].m_Pad1 = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                this.bodyArray[i].m_Pad2 = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
            }
            this.colorCount = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_ColorArray = new PentLightColor[this.colorCount];
            for (i = 0; i < this.colorCount; i++)
            {
                String name = HLMessageTypeHelper.extractString(this.data, ref this.startIndex);
                RgbColor color = HLMessageTypeHelper.extractColor(this.data, ref this.startIndex);
                this.m_ColorArray[i] = new PentLightColor(name, color);
            }
            this.m_PumpCircArray = new byte[8];
            for (i = 0; i < 8; i++)
            {
                this.m_PumpCircArray[i] = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
            }
            this.m_InterfaceTabFlags = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_ShowAlarms = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
        }

        public int getM_ControllerID()
        {
            return this.m_ControllerID;
        }

        public byte getM_DegC()
        {
            return this.m_DegC;
        }

        public byte getM_ControllerType()
        {
            return this.m_ControllerType;
        }

        public byte getM_HWType()
        {
            return this.m_HWType;
        }

        public byte getM_ControllerData()
        {
            return this.m_ControllerData;
        }

        public int getM_EquipFlags()
        {
            return this.m_EquipFlags;
        }

        public String getM_genCircuitName()
        {
            return this.m_genCircuitName;
        }

        public int getM_CircuitCount()
        {
            return this.m_CircuitCount;
        }

        public byte[] getMinSetPoint()
        {
            return this.m_MinSetPoint;
        }

        public byte[] getMaxSetPoint()
        {
            return this.m_MaxSetPoint;
        }

        public BodyDataStructure[] getBodyArray()
        {
            return this.bodyArray;
        }

        public int getM_interfaceTabFlags()
        {
            return this.m_InterfaceTabFlags;
        }

        public int getM_showAlarms()
        {
            return this.m_ShowAlarms;
        }

        public PentLightColor[] getColorLightList()
        {
            return this.m_ColorArray;
        }

        public int getColorCount()
        {
            return this.colorCount;
        }

        public int getPumpCirCount()
        {
            return 8;
        }

        public byte[] getPumpCirList()
        {
            return this.m_PumpCircArray;
        }
    }
}
