namespace ScreenLogicConnect.Messages;

public class ChallengeString : HLMessage
{
    public string? ChallengeStr { get; private set; }

    internal override short QueryId => 14;

    internal ChallengeString(short senderID = 0)
            : base(senderID)
    {
    }

    protected override void Decode()
    {
        ChallengeStr = HLMessageTypeHelper.ExtractString(data);
    }
}
