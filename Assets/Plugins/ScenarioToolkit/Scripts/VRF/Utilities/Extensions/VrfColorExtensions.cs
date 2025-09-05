using UnityEngine;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с цветами
    /// </summary>
    public static class VrfColorExtensions
    {
        public static Color GetNewWithR(this Color c, float r) => new(r, c.g, c.b, c.a);
        public static Color GetNewWithG(this Color c, float g) => new(c.r, g, c.b, c.a);
        public static Color GetNewWithB(this Color c, float b) => new(c.r, c.g, b, c.a);
        public static Color GetNewWithA(this Color c, float a) => new(c.r, c.g, c.b, a);
        public static Color GetNewWithRGB(this Color c, float r, float g, float b) => new(r, g, b, c.a);
        
        public static Color32 GetNewWithR(this Color32 c, byte r) => new(r, c.g, c.b, c.a);
        public static Color32 GetNewWithG(this Color32 c, byte g) => new(c.r, g, c.b, c.a);
        public static Color32 GetNewWithB(this Color32 c, byte b) => new(c.r, c.g, b, c.a);
        public static Color32 GetNewWithA(this Color32 c, byte a) => new(c.r, c.g, c.b, a);
        public static Color32 GetNewWithRGB(this Color32 c, byte r, byte g, byte b) => new(r, g, b, c.a);
    }
}