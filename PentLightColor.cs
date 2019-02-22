using System;

namespace ScreenLogicConnect
{
    public class PentLightColor
    {
        public RgbColor color { get; private set; }
        public string name { get; private set; }

        public PentLightColor(string inName, RgbColor inColor)
        {
            name = inName;
            color = inColor;
        }

        public override string ToString()
        {
            return $"Name: {name}, color: {color}";
        }
    }
}
