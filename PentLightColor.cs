using System;

namespace ScreenLogicConnect
{
    public class PentLightColor
    {
        public RgbColor color { get; private set; }
        public string name { get; private set; }

        public PentLightColor(string name, RgbColor color)
        {
            this.name = name;
            this.color = color;
        }

        public override string ToString()
        {
            return $"Name: {name}, color: {color}";
        }
    }
}
