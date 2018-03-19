namespace ScreenLogicConnect.Messages
{
    public class GetPoolStatus : HLMessage
    {
        //private CircuitUpdateDataStructure[] circuitArray;
        private int m_AirTemp;
        private int m_Alarms;
        private int m_BodiesCount;
        private int m_CircuitCount;
        private sbyte m_CleanerDelay;
        private int[] m_CoolSetPoint;
        private int[] m_CurrentTemp;
        private sbyte m_FreezeMode;
        private int[] m_HeatMode;
        private int[] m_HeatStatus;
        private int m_ORP;
        private int m_ORPTank;
        private int m_Ok;
        private int m_PH;
        private int m_PHTank;
        private sbyte m_Padding;
        private sbyte m_PoolDelay;
        private sbyte m_Remotes;
        private int m_SaltPPM;
        private int m_Saturation;
        private int[] m_SetPoint;
        private sbyte m_SpaDelay;

        public static GetPoolStatus QUERY(short senderID)
        {
            return new GetPoolStatus(senderID, (short)12526);
        }

        private GetPoolStatus(short senderID, short msgID)
                : base(senderID, msgID)
        {
        }

        public GetPoolStatus(sbyte[] header, sbyte[] data)
                : base(header, data)
        {
        }

        public GetPoolStatus(HLMessage msg)
                : base(msg)
        {
        }

        public override sbyte[] asByteArray()
        {
            putInteger(0);
            return base.asByteArray();
        }

        protected override void decode()
        {
            int i;
            this.startIndex = 0;
            this.m_Ok = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_FreezeMode = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_Remotes = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_PoolDelay = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_SpaDelay = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_CleanerDelay = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_Padding = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_Padding = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_Padding = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
            this.startIndex++;
            this.m_AirTemp = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_BodiesCount = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            if (this.m_BodiesCount > 2)
            {
                this.m_BodiesCount = 2;
            }
            this.m_CurrentTemp = new int[2];
            this.m_HeatStatus = new int[2];
            this.m_SetPoint = new int[2];
            this.m_CoolSetPoint = new int[2];
            this.m_HeatMode = new int[2];
            for (i = 0; i < this.m_BodiesCount; i++)
            {
                int bodyType = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
                if (bodyType < 0 || bodyType >= 2)
                {
                    bodyType = 0;
                }
                this.m_CurrentTemp[bodyType] = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
                this.m_HeatStatus[bodyType] = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
                this.m_SetPoint[bodyType] = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
                this.m_CoolSetPoint[bodyType] = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
                this.m_HeatMode[bodyType] = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
            }
            int m_CircuitCount = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            //this.circuitArray = new CircuitUpdateDataStructure[m_CircuitCount];
            for (i = 0; i < m_CircuitCount; i++)
            {
                //this.circuitArray[i] = new CircuitUpdateDataStructure();
                //this.circuitArray[i].id = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
                //this.circuitArray[i].state = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
                this.startIndex += 4;
                //this.circuitArray[i].colorSet = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                //this.circuitArray[i].colorPos = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                //this.circuitArray[i].colorStagger = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
                //this.circuitArray[i].delay = ByteHelper.getUnsignedByteFromByteArray(this.data, this.startIndex);
                this.startIndex++;
            }
            this.m_PH = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_ORP = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_Saturation = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_SaltPPM = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_PHTank = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_ORPTank = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
            this.m_Alarms = ByteHelper.getIntFromByteArrayLittleEndian(this.data, this.startIndex);
            this.startIndex += 4;
        }

        public int getM_Ok()
        {
            return this.m_Ok;
        }

        public sbyte getM_FreezeMode()
        {
            return this.m_FreezeMode;
        }

        public sbyte getM_Remotes()
        {
            return this.m_Remotes;
        }

        public sbyte getM_PoolDelay()
        {
            return this.m_PoolDelay;
        }

        public sbyte getM_SpaDelay()
        {
            return this.m_SpaDelay;
        }

        public sbyte getM_CleanerDelay()
        {
            return this.m_CleanerDelay;
        }

        public sbyte getM_Padding()
        {
            return this.m_Padding;
        }

        public int getM_AirTemp()
        {
            return this.m_AirTemp;
        }

        public int getM_BodiesCount()
        {
            return this.m_BodiesCount;
        }

        public int[] getM_CurrentTemp()
        {
            return this.m_CurrentTemp;
        }

        public int[] getM_HeatStatus()
        {
            return this.m_HeatStatus;
        }

        public int[] getM_SetPoint()
        {
            return this.m_SetPoint;
        }

        public int[] getM_CoolSetPoint()
        {
            return this.m_CoolSetPoint;
        }

        public int[] getM_HeatMode()
        {
            return this.m_HeatMode;
        }

        public int getM_PH()
        {
            return this.m_PH;
        }

        public int getM_ORP()
        {
            return this.m_ORP;
        }

        public int getM_Saturation()
        {
            return this.m_Saturation;
        }

        public int getM_SaltPPM()
        {
            return this.m_SaltPPM;
        }

        public int getM_PHTank()
        {
            return this.m_PHTank;
        }

        public int getM_ORPTank()
        {
            return this.m_ORPTank;
        }

        public int getM_Alarms()
        {
            return this.m_Alarms;
        }
        /*
            public CircuitUpdateDataStructure[] getCircuitArray()
            {
                return this.circuitArray;
            }
        */
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
    }
}
