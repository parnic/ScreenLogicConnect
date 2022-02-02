namespace ScreenLogicConnect.Messages;

public class GetMode : HLMessage
{
    internal override short QueryId => 110;

    public GetMode()
    {

    }

    internal GetMode(short senderID = 0)
        : base(senderID)
    {
    }
}
