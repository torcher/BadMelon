using BadMelon.Data.Entities;
using System;

namespace BadMelon.Tests.Fixtures.Entities
{
    public class IngredientTypeFixture
    {
        private string _name;
        private Guid _id = Guid.Empty;

        public IngredientTypeFixture(string name)
        {
            _name = name;
        }

        public IngredientTypeFixture WithID(Guid id)
        {
            _id = id;
            return this;
        }

        public IngredientType Build()
        {
            return new IngredientType { Name = _name };
        }
    }
}