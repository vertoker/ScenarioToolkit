using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scenario.Utilities.Extensions
{
    public static class StringLevenshteinDistanceExtension
    {
        private static List<int> DCurrent = new();
        private static List<int> DMinus1 = new();
        private static List<int> DMinus2 = new();
        
        /// <summary>
        /// Computes the Damerau-Levenshtein Distance between two strings, represented as arrays of
        /// integers, where each integer represents the code point of a character in the source string.
        /// Includes an optional threshhold which can be used to indicate the maximum allowable distance.
        /// </summary>
        /// <param name="source">An array of the code points of the first string</param>
        /// <param name="target">An array of the code points of the second string</param>
        /// <param name="threshold">Maximum allowable distance</param>
        /// <returns>Int.MaxValue if threshhold exceeded; otherwise the Damerau-Leveshteim distance between the strings</returns>
        public static int LevenshteinDistance(this string source, string target, int threshold)
        {
            var length1 = source.Length;
            var length2 = target.Length;

            // Return trivial case - difference in string lengths exceeds threshhold
            if (Mathf.Abs(length1 - length2) > threshold)
                return int.MaxValue;

            // Ensure arrays [i] / length1 use shorter length 
            if (length1 > length2)
            {
                (target, source) = (source, target);
                (length1, length2) = (length2, length1);
            }

            var maxi = length1;
            var maxj = length2;
            
            DCurrent.Clear();
            DMinus1.Clear();
            DMinus2.Clear();
            
            DCurrent.AddRange(Enumerable.Repeat(0, maxi + 1));
            DMinus1.AddRange(Enumerable.Repeat(0, maxi + 1));
            DMinus2.AddRange(Enumerable.Repeat(0, maxi + 1));

            for (var i = 0; i <= maxi; i++)
            {
                DCurrent[i] = i;
            }

            int jm1 = 0, im1 = 0, im2 = -1;

            for (var j = 1; j <= maxj; j++)
            {
                // Rotate
                var dSwap = DMinus2;
                DMinus2 = DMinus1;
                DMinus1 = DCurrent;
                DCurrent = dSwap;

                // Initialize
                var minDistance = int.MaxValue;
                DCurrent[0] = j;
                im1 = 0;
                im2 = -1;

                for (var i = 1; i <= maxi; i++)
                {
                    var cost = source[im1] == target[jm1] ? 0 : 1;

                    var del = DCurrent[im1] + 1;
                    var ins = DMinus1[i] + 1;
                    var sub = DMinus1[im1] + cost;

                    //Fastest execution for min value of 3 integers
                    var min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

                    if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
                        min = Mathf.Min(min, DMinus2[im2] + cost);

                    DCurrent[i] = min;
                    if (min < minDistance)
                    {
                        minDistance = min;
                    }

                    im1++;
                    im2++;
                }

                jm1++;
                if (minDistance > threshold)
                {
                    return int.MaxValue;
                }
            }

            var result = DCurrent[maxi];
            return result > threshold ? int.MaxValue : result;
        }
    }
}