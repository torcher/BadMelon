using System;

namespace BadMelon.Data.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToRecipeFormat(this TimeSpan ts)
        {
            return $"{(ts.Days * 24) + ts.Hours}:{ts.Minutes}:{ts.Seconds}";
        }

        public static TimeSpan FromRecipeFormat(this string s)
        {
            var split = s.Split(':');
            try
            {
                if (split.Length != 3) throw new Exception();

                int hh = int.Parse(split[0]);
                int mm = int.Parse(split[1]);
                int ss = int.Parse(split[2]);

                if (hh < 0 || mm < 0 || ss < 0) throw new Exception();

                return new TimeSpan(hh, mm, ss);
            }
            catch (Exception)
            {
                throw new ArgumentException($"{s} is an invalid input");
            }
        }
    }
}