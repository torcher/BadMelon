namespace BadMelon.Data.Domain
{
    public interface IIngredientContainer
    {
        bool HasIngredientType(IngredientType type);

        bool HasEnoughIngredient(Ingredient ingredient);
    }
}