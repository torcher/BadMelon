﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.Entities
{
    public class Recipe : Entity
    {
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Step> Steps { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}