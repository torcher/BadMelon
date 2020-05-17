using System;
using System.Diagnostics.CodeAnalysis;

namespace BadMelon.RecipeMath
{
    public class IngredientType : IEquatable<IngredientType>
    {
        public string Name { get; private set; }

        public IngredientType()
        {
        }

        public IngredientType(string name)
        {
            Name = name;
        }

        public override bool Equals([AllowNull] object other)
        {
            //Do null comparison
            if (this is null)
                if (other is null)
                    return true;
                else
                    return false;
            if (other is null) return false;

            //Do type comparison
            if (other.GetType() != typeof(IngredientType)) return false;

            //Compare names
            return this.Name.ToLower() == ((IngredientType)other).Name.ToLower();
        }

        public bool Equals([AllowNull] IngredientType other) => this.Equals(other);

        public static bool operator ==(IngredientType a, IngredientType b) => a.Equals(b);

        public static bool operator !=(IngredientType a, IngredientType b) => !(a == b);

        public override int GetHashCode() => Name.GetHashCode();
    }
}