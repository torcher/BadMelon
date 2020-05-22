using System;

namespace BadMelon.API.DTOs
{
    public class Step
    {
        public Guid ID { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public string CookTime { get; set; }
        public string PrepTime { get; set; }
    }
}