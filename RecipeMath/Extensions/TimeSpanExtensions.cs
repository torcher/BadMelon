using System;

namespace BadMelon.RecipeMath.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToFormattedString(this TimeSpan ts) => ts.ToString("g");
    }
}