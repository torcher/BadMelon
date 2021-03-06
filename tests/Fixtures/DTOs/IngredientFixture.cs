﻿using BadMelon.Data.DTOs;
using System;

namespace BadMelon.Tests.Fixtures.DTOs
{
    public class IngredientFixture
    {
        private Ingredient _ingredient;

        public IngredientFixture(double weight, Guid typeId)
        {
            _ingredient = new Ingredient { Weight = weight, TypeID = typeId };
        }

        public Ingredient Build()
        {
            return _ingredient;
        }
    }
}