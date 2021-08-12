using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine {
    public static class Mathf {
        public const float PI = (float)Math.PI;
        /// <summary>
        /// Returns where between a and b the given value sits
        /// </summary>
        /// <param name="a">The lower bound value</param>
        /// <param name="b">The higher bound value</param>
        /// <param name="value">The value to calculate for</param>
        /// <returns>How far along the path the value is between a and b</returns>
        public static float InverseLerp(float a, float b, float value) => (value - a) / (b - a);
    }
}
