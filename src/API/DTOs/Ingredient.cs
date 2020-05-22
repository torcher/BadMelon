using System;

namespace BadMelon.API.DTOs
{
    public class Ingredient
    {
        public Guid ID { get; set; }
        public double Weight { get; set; }
        public Guid TypeID { get; set; }
        public string Type { get; set; }
    }
}