using System;
using System.IO;

namespace ScreenLogicConnect.Messages
{
    public class ChallengeString : HLMessage
    {
        public string ChallengeStr { get; private set; }

        public const short HLM_CLIENT_CHALLENGE = 14;

        public static ChallengeString QUERY(short senderID)
        {
            return new ChallengeString(senderID, HLM_CLIENT_CHALLENGE);
        }

        private ChallengeString(short senderID, short msgID)
                : base(senderID, msgID)
        {
        }

        public ChallengeString(ReadOnlySpan<byte> header, ReadOnlySpan<byte> data)
                : base(header, data)
        {
        }

        public ChallengeString(HLMessage msg)
                : base(msg)
        {
        }

        protected override void Decode()
        {
            ChallengeStr = HLMessageTypeHelper.ExtractString(data);
        }
    }
}
