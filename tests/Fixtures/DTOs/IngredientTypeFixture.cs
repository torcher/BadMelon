using BadMelon.Data.DTOs;
using System;

namespace BadMelon.Tests.Fixtures.DTOs
{
    public class IngredientTypeFixture
    {
        private Guid _id = Guid.Empty;
        private string _name;

        public IngredientTypeFixture(string name)
        {
            _name = name;
        }

        public IngredientTypeFixture WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public IngredientType Build()
        {
            return new IngredientType
            {
                ID = _id,
                Name = _name
            };
        }
    }
}