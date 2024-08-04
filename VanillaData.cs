using Colossal.Json;
using UnityEngine;

namespace BoundaryLinesModifier
{
    public class VanillaData
    {
        public int Width { get; }
        public int Length { get; }
        public Color CityBorderColor { get; }
        public Color MapBorderColor { get; }

        public VanillaData()
        {
            Width = 10;
            Length = 80;
            CityBorderColor = ColorParser.ParseColor("RGBA(1.000, 1.000, 1.000, 0.741)");
            MapBorderColor = ColorParser.ParseColor("RGBA(1.000, 0.628, 0.000, 0.250)");
        }
    }
}
