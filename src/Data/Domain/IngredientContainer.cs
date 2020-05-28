using System.Collections.Generic;

namespace BadMelon.Data.Domain
{
    public abstract class IngredientContainer : IIngredientContainer
    {
        public List<Ingredient> Ingredients { get; protected set; }

        protected IngredientContainer()
        {
            Ingredients = new List<Ingredient>();
        }

        public Ingredient this[int index] { get => Ingredients[index]; set => Ingredients[index] = value; }

        public int Count => Ingredients.Count;

        public bool IsReadOnly => false;

        public void Add(Ingredient item)
        {
            Ingredients.Add(item);
        }

        public void Clear()
        {
            Ingredients.Clear();
        }

        public bool Contains(Ingredient item)
        {
            for (int i = 0; i < Ingredients.Count; i++)
                if (Ingredients[i] == item)
                    return true;
            return false;
        }

        public void CopyTo(Ingredient[] array, int arrayIndex)
        {
            Ingredients.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Ingredient> GetEnumerator() => Ingredients.GetEnumerator();

        public bool HasEnoughIngredient(Ingredient ingredient)
        {
            foreach (var i in Ingredients)
                if (i >= ingredient)
                    return true;
            return false;
        }

        public bool HasIngredientType(IngredientType type)
        {
            foreach (var i in Ingredients)
                if (i.Type == type)
                    return true;
            return false;
        }

        public int IndexOf(Ingredient item)
        {
            for (int i = 0; i < Ingredients.Count; i++)
                if (item == Ingredients[i])
                    return i;
            return -1;
        }

        public void Insert(int index, Ingredient item)
        {
            Ingredients.Insert(index, item);
        }

        public bool Remove(Ingredient item)
        {
            return Ingredients.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Ingredients.RemoveAt(index);
        }
    }
}