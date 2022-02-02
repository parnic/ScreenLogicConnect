namespace ScreenLogicConnect;

public class RgbColor
{
    public byte R { get; private set; }
    public byte G { get; private set; }
    public byte B { get; private set; }

    public RgbColor(byte inR, byte inG, byte inB)
    {
        R = inR;
        G = inG;
        B = inB;
    }

    public override string ToString()
    {
        return $"R {R}, G {G}, B {B}";
    }
}
