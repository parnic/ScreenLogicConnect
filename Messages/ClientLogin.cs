using System;

namespace ScreenLogicConnect.Messages
{
    public class ClientLogin : HLMessage
    {
        private sbyte[] m_byteArray;
        private int m_connectionType;
        private int m_int;
        private int m_procID;
        private int m_schema;
        private String m_version;

        public static ClientLogin QUERY(short senderID)
        {
            return new ClientLogin(senderID, (short)27);
        }

        public ClientLogin(short senderID, short msgID)
                : base(senderID, msgID)
        {
        }

        public override sbyte[] asByteArray()
        {
            putInteger(this.m_schema);
            putInteger(this.m_connectionType);
            putString(this.m_version);
            putByteArray(this.m_byteArray);
            putInteger(this.m_procID);
            return base.asByteArray();
        }

        public void set_schema(int m_schema)
        {
            this.m_schema = m_schema;
        }

        public int get_schema()
        {
            return this.m_schema;
        }

        public void set_connectionType(int m_connectionType)
        {
            this.m_connectionType = m_connectionType;
        }

        public int get_connectionType()
        {
            return this.m_connectionType;
        }

        public void set_version(String m_version)
        {
            this.m_version = m_version;
        }

        public String get_version()
        {
            return this.m_version;
        }

        public void set_int(int m_int)
        {
            this.m_int = m_int;
        }

        public int get_int()
        {
            return this.m_int;
        }

        public void set_byteArray(sbyte[] m_byteArray)
        {
            this.m_byteArray = m_byteArray;
        }

        public sbyte[] get_byteArray()
        {
            return this.m_byteArray;
        }

        public void set_procID(int m_procID)
        {
            this.m_procID = m_procID;
        }

        public int get_procID()
        {
            return this.m_procID;
        }
    }
}
