namespace BadMelon.RecipeMath
{
    public class Ingredient
    {
        public IngredientType Type { get; }
        public double Weight { get; set; }

        public Ingredient(double weight, string Type) : this(weight, new IngredientType(Type))
        {
        }

        public Ingredient(string Type) : this(0d, new IngredientType(Type))
        {
        }

        public Ingredient(IngredientType type) : this(0d, type)
        {
        }

        public Ingredient(double weight, IngredientType type)
        {
            if (weight < 0d)
                Weight = 0d;
            else
                Weight = weight;
            Type = type;
        }

        public static Ingredient operator +(Ingredient a, Ingredient b)
        {
            CheckType(a, b);
            return new Ingredient(a.Weight + b.Weight, a.Type);
        }

        public static Ingredient operator -(Ingredient a, Ingredient b)
        {
            CheckType(a, b);
            return new Ingredient(a.Weight - b.Weight, a.Type);
        }

        public static bool operator ==(Ingredient a, Ingredient b)
        {
            if (!SafeCheckType(a, b)) return false;
            return a.Weight == b.Weight;
        }

        public static bool operator !=(Ingredient a, Ingredient b) => !(a == b);

        public static bool operator >(Ingredient a, Ingredient b)
        {
            if (!SafeCheckType(a, b)) return false;
            return a.Weight > b.Weight;
        }

        public static bool operator <(Ingredient a, Ingredient b)
        {
            if (!SafeCheckType(a, b)) return false;
            return a.Weight < b.Weight;
        }

        public static bool operator >=(Ingredient a, Ingredient b) => a == b || a > b;

        public static bool operator <=(Ingredient a, Ingredient b) => a == b || a < b;

        private static void CheckType(Ingredient a, Ingredient b)
        {
            if (a.Type != b.Type) throw new IngredientMismatchException($"Cannot convert {a.Type.Name} to {b.Type.Name}");
        }

        private static bool SafeCheckType(Ingredient a, Ingredient b)
        {
            try { CheckType(a, b); }
            catch (IngredientMismatchException) { return false; }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return this is null;
            }

            if (obj.GetType() != typeof(Ingredient)) return false;

            return this == (Ingredient)obj;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Type.GetHashCode();
            hash = (hash * 7) + Weight.GetHashCode();
            return hash;
        }
    }
}