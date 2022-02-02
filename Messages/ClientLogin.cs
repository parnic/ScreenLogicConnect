using System;
using System.IO;

namespace ScreenLogicConnect.Messages
{
    public class ClientLogin : HLMessage
    {
        public byte[]? m_password;
        public int m_connectionType;
        public int m_int;
        public int m_procID;
        public int m_schema;
        public string m_version = "ScreenLogicConnect library";

        public const short HLM_CLIENT_LOGIN = 27;

        public static ClientLogin QUERY(short senderID)
        {
            return new ClientLogin(senderID, HLM_CLIENT_LOGIN);
        }

        public ClientLogin(short senderID, short msgID)
                : base(senderID, msgID)
        {
        }

        public override Span<byte> AsByteArray()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(m_schema);
                    bw.Write(m_connectionType);
                    bw.WritePrefixLength(m_version);
                    bw.WritePrefixLength(m_password);
                    bw.Write(m_procID);
                }

                data = ms.ToArray();
            }

            return base.AsByteArray();
        }
    }
}
