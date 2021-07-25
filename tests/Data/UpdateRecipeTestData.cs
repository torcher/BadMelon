using BadMelon.Data.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BadMelon.Tests.Data
{
    public class UpdateRecipeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Type = "Water", Weight = 2d },
                    new Ingredient { TypeID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Type = "Corn", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Mix Corn and Water" },
                    new Step { Order = 2, Text = "Serve" }
                }
            } };

            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Type = "Water", Weight = 3d },
                    new Ingredient { TypeID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Type = "Corn", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Mix Corn and Water" },
                    new Step { Order = 2, Text = "Serve" }
                }
            } };

            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Type = "Corn", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Mix Corn and Water" },
                    new Step { Order = 2, Text = "Serve" }
                }
            } };

            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Type = "Water", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Mix Corn and Water" },
                    new Step { Order = 2, Text = "Serve" }
                }
            } };

            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Type = "Water", Weight = 2d } ,
                    new Ingredient { TypeID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Type = "Corn", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Mix Corn and Water" },
                    new Step { Order = 2, Text = "Serve" }
                }
            } };

            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Type = "Water", Weight = 2d },
                    new Ingredient { TypeID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Type = "Corn", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Mix Corn and Water" },
                    new Step { Order = 2, Text = "Serve cold" }
                }
            } };

            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Type = "Water", Weight = 2d },
                    new Ingredient { TypeID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Type = "Corn", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Serve cold" },
                    new Step { Order = 2, Text = "Mix Corn and Water" }
                }
            } };

            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Type = "Water", Weight = 2d },
                    new Ingredient { TypeID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Type = "Corn", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Mix Corn and Water" }
                }
            } };

            yield return new object[]{ new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                Name = "Cold Corn Soupa",
                Ingredients = new List<Ingredient> {
                    new Ingredient { TypeID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Type = "Water", Weight = 2d },
                    new Ingredient { TypeID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Type = "Corn", Weight = 1d }
                },
                Steps = new List<Step> {
                    new Step { Order = 1, Text = "Serve" },
                    new Step { Order = 2, Text = "Mix Corn and Water" }
                }
            } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}