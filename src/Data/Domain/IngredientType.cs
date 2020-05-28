using System;
using System.Diagnostics.CodeAnalysis;

namespace BadMelon.Data.Domain
{
    public class IngredientType : IEquatable<IngredientType>
    {
        public Guid ID { get; set; }
        public string Name { get; private set; }

        public IngredientType(Guid id, string name) : this(name)
        {
            ID = id;
        }

        public IngredientType(string name)
        {
            Name = name;
        }

        public bool Equals([AllowNull] IngredientType other)
        {
            if (this is null || other is null) return false;
            if (this.Name is null) return false;
            return this.Name.ToLower() == other.Name.ToLower();
        }

        public static bool operator ==(IngredientType a, IngredientType b) => a.Equals(b);

        public static bool operator !=(IngredientType a, IngredientType b) => !(a.Equals(b));

        public override int GetHashCode() => Name.GetHashCode();
    }
}