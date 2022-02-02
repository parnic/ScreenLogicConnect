namespace ScreenLogicConnect;

public class PentLightColor
{
    public RgbColor Color { get; private set; }
    public string Name { get; private set; }

    public PentLightColor(string inName, RgbColor inColor)
    {
        Name = inName;
        Color = inColor;
    }

    public override string ToString()
    {
        return $"Name: {Name}, color: {Color}";
    }
}
