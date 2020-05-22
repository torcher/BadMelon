namespace BadMelon.RecipeMath
{
    public interface IIngredientContainer
    {
        bool HasIngredientType(IngredientType type);

        bool HasEnoughIngredient(Ingredient ingredient);
    }
}