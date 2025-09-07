// Credits
// Based on https://easings.net/
// Created by vertoker (09.06.2022)

using UnityEngine;

namespace SimpleUI.Extensions
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static class Easings
    {
        /// <summary> Types of smoothness functions </summary>
        public enum Type
        {
            Linear = 0,      Constant = 1,                        // Usual
            InSine = 2,      OutSine = 3,      InOutSine = 4,     // Sin functions
            InQuad = 5,      OutQuad = 6,      InOutQuad = 7,     // 2-power functions (square)
            InCubic = 8,     OutCubic = 9,     InOutCubic = 10,   // 3-power functions (cube)
            InQuart = 11,    OutQuart = 12,    InOutQuart = 13,   // 4-power functions (tesseract)
            InQuint = 14,    OutQuint = 15,    InOutQuint = 16,   // 5-power functions
            InExpo = 17,     OutExpo = 18,     InOutExpo = 19,    // Exponentially functions
            InCirc = 20,     OutCirc = 21,     InOutCirc = 22,    // Circle functions
            InBack = 23,     OutBack = 24,     InOutBack = 25,    // Inertial functions
            InElastic = 26,  OutElastic = 27,  InOutElastic = 28  // Elastic functions
        }
        
        private const float C1 = 1.70158f;
        private const float C2 = C1 * 1.525f;
        private const float C3 = C1 + 1f;
        private const float C4 = 2 * Mathf.PI / 3;
        private const float C5 = 2 * Mathf.PI / 4.5f;

        /// <summary> Modify progress (0-1) by easings functions </summary>
        /// <param name="x">Progress (from 0 to 1)</param>
        /// <param name="easing">Easing function</param>
        /// <returns></returns>
        public static float GetEasing(float x, Type easing)
        {
            return easing switch
            {
                Type.Linear => x,
                Type.Constant => Mathf.Floor(x),
                
                Type.InSine => 1 - Mathf.Cos((x * Mathf.PI) / 2),
                Type.OutSine => Mathf.Sin((x * Mathf.PI) / 2),
                Type.InOutSine => -(Mathf.Cos(Mathf.PI * x) - 1) / 2,
                
                Type.InQuad => x * x,
                Type.OutQuad => 1 - Mathf.Pow(1 - x, 2),
                Type.InOutQuad => x < 0.5f ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2,
                
                Type.InCubic => x * x * x,
                Type.OutCubic => 1 - Mathf.Pow(1 - x, 3),
                Type.InOutCubic => x < 0.5f ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2,
                
                Type.InQuart => x * x * x * x,
                Type.OutQuart => 1 - Mathf.Pow(1 - x, 4),
                Type.InOutQuart => x < 0.5f ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2,
                
                Type.InQuint => x * x * x * x * x,
                Type.OutQuint => 1 - Mathf.Pow(1 - x, 5),
                Type.InOutQuint => x < 0.5f ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2,
                
                Type.InExpo => x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10),
                Type.OutExpo => x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x),
                Type.InOutExpo => x == 0 ? 0 : x == 1 ? 1 :
                    x < 0.5f ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2,
                
                Type.InCirc => 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)),
                Type.OutCirc => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2)),
                Type.InOutCirc => x < 0.5f
                    ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
                    : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2,
                
                Type.InBack => C3 * x * x * x - C1 * x * x,
                Type.OutBack => 1 + C3 * Mathf.Pow(x - 1, 3) + C1 * Mathf.Pow(x - 1, 2),
                Type.InOutBack => x < 0.5f
                    ? (Mathf.Pow(2 * x, 2) * ((C2 + 1) * 2 * x - C2)) / 2
                    : (Mathf.Pow(2 * x - 2, 2) * ((C2 + 1) * (x * 2 - 2) + C2) + 2) / 2,
                
                Type.InElastic => x == 0 ? 0 :
                    x == 1 ? 1 : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * C4),
                Type.OutElastic => x == 0 ? 0 :
                    x == 1 ? 1 : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * C4) + 1,
                Type.InOutElastic => x == 0 ? 0 :
                    x == 1 ? 1 :
                    x < 0.5f ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * C5)) / 2 :
                    (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * C5)) / 2 + 1,
                
                _ => 0
            };
        }
    }
}