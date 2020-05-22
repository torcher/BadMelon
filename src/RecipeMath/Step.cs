using BadMelon.RecipeMath.Extensions;
using System;
using System.Text.Json.Serialization;

namespace BadMelon.RecipeMath
{
    public class Step
    {
        public string Text { get; set; }
        public int Order { get; set; }

        [JsonIgnore]
        public TimeSpan CookTimeSpan { get; set; }

        public string CookTime { get => CookTimeSpan.ToFormattedString(); }

        [JsonIgnore]
        public TimeSpan PrepTimeSpan { get; set; }

        public string PrepTime { get => PrepTimeSpan.ToFormattedString(); }
    }
}