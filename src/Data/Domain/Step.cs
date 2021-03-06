﻿using BadMelon.Data.Extensions;
using System;
using System.Text.Json.Serialization;

namespace BadMelon.Data.Domain
{
    public class Step
    {
        public string Text { get; set; }
        public int Order { get; set; }

        [JsonIgnore]
        public TimeSpan CookTimeSpan { get; set; }

        public string CookTime { get => CookTimeSpan.ToRecipeFormat(); }

        [JsonIgnore]
        public TimeSpan PrepTimeSpan { get; set; }

        public string PrepTime { get => PrepTimeSpan.ToRecipeFormat(); }
    }
}