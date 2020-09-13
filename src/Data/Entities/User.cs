using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BadMelon.Data.Entities
{
    public class User : IdentityUser
    {
        public virtual Guid EmailVerificationCode { get; set; }
        public DateTime EmailVerificationCreate { get; set; }
        public DateTime EmailVerified { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}