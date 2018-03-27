namespace ScreenLogicConnect
{
    public class RgbColor
    {
        public byte r { get; private set; }
        public byte g { get; private set; }
        public byte b { get; private set; }

        public RgbColor(byte inR, byte inG, byte inB)
        {
            r = inR;
            g = inG;
            b = inB;
        }

        public override string ToString()
        {
            return $"R {r}, G {g}, B {b}";
        }
    }
}
