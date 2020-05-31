using BadMelon.Data.Entities;
using System;

namespace BadMelon.Tests.Data.Fixtures
{
    public class IngredientFixture
    {
        private double _weight = 1.0;
        private Guid _typeId;

        public IngredientFixture(Guid typeId)
        {
            _typeId = typeId;
        }

        public IngredientFixture WithWeight(double weight)
        {
            _weight = weight;
            return this;
        }

        public Ingredient Build()
        {
            return new Ingredient
            {
                Weight = _weight,
                IngredientTypeID = _typeId
            };
        }
    }
}