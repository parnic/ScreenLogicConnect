namespace ScreenLogicConnect.Messages
{
    public class GetMode : HLMessage
    {
        public const short HLM_MODE_GETMODEQ = 110;

        public static GetMode QUERY(short senderID)
        {
            return new GetMode(senderID, (short)HLM_MODE_GETMODEQ);
        }

        private GetMode(short senderID, short msgID)
            : base(senderID, msgID)
        {
        }

        public GetMode(sbyte[] header, sbyte[] data)
            : base(header, data)
        {
        }

        public GetMode(HLMessage msg)
            : base(msg)
        {
        }
    }
}
