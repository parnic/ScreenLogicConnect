using System;

namespace ScreenLogicConnect.Messages
{
    public class ChallengeString : HLMessage
    {
        private String challengeString;

        public static ChallengeString QUERY(short senderID)
        {
            return new ChallengeString(senderID, (short)14);
        }

        private ChallengeString(short senderID, short msgID)
                : base(senderID, msgID)
        {
        }

        public ChallengeString(sbyte[] header, sbyte[] data)
                : base(header, data)
        {
        }

        public ChallengeString(HLMessage msg)
                : base(msg)
        {
        }

        protected override void decode()
        {
            this.startIndex = 0;
            this.challengeString = HLMessageTypeHelper.extractString(this.data, ref startIndex);
        }

        public String getChallengeString()
        {
            return this.challengeString;
        }
    }
}
