namespace ScreenLogicConnect.Messages;

public class ClientLogin : HLMessage
{
    public byte[]? Password;
    public int ConnectionType;
    public int Int;
    public int ProcID;
    public int Schema;
    public string Version = "ScreenLogicConnect library";

    private const short LoginQueryId = 27;
    internal override short QueryId => LoginQueryId;

    internal const short LoginAnswerId = LoginQueryId + 1;

    public ClientLogin()
    {
    }

    internal ClientLogin(short senderID = 0)
            : base(senderID)
    {
    }

    public override Span<byte> AsByteArray()
    {
        using (var ms = new MemoryStream())
        {
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(Schema);
                bw.Write(ConnectionType);
                bw.WritePrefixLength(Version);
                bw.WritePrefixLength(Password);
                bw.Write(ProcID);
            }

            data = ms.ToArray();
        }

        return base.AsByteArray();
    }
}
