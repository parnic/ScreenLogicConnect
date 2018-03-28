﻿using System;
using System.IO;

namespace ScreenLogicConnect.Messages
{
    public class ChallengeString : HLMessage
    {
        public String challengeString { get; private set; }

        public const short HLM_CLIENT_CHALLENGE = 14;

        public static ChallengeString QUERY(short senderID)
        {
            return new ChallengeString(senderID, HLM_CLIENT_CHALLENGE);
        }

        private ChallengeString(short senderID, short msgID)
                : base(senderID, msgID)
        {
        }

        public ChallengeString(byte[] header, byte[] data)
                : base(header, data)
        {
        }

        public ChallengeString(HLMessage msg)
                : base(msg)
        {
        }

        protected override void decode()
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(dataByteStream))
                {
                    challengeString = HLMessageTypeHelper.extractString(br);
                }
            }
        }
    }
}
