using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BadMelon.Data.Entities
{
    public class Recipe
    {
        public Guid ID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
