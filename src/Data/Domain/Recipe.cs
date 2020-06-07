using BadMelon.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BadMelon.Data.Domain
{
    public class Recipe : IngredientContainer
    {
        public Guid ID { get; private set; }
        public string Name { get; set; }
        public List<Step> Steps { get; private set; }

        public Recipe(string name) : this(Guid.NewGuid(), name)
        {
        }

        public Recipe(Guid id, string name) : this(id, name, new List<Ingredient>(), new List<Step>())
        {
        }

        public Recipe(Guid id, string name, List<Ingredient> ingredients, List<Step> steps)
        {
            Name = name;
            ID = id;
            Ingredients = ingredients;
            Steps = steps;
        }

        [JsonIgnore]
        public TimeSpan PrepTimespan { get => new TimeSpan(Steps.Select(ts => ts.PrepTimeSpan.Ticks).Sum()); }

        public string PrepTime { get => PrepTimespan.ToRecipeFormat(); }

        [JsonIgnore]
        public TimeSpan CookTimespan { get => new TimeSpan(Steps.Select(ts => ts.CookTimeSpan.Ticks).Sum()); }

        public string CookTime { get => PrepTimespan.ToRecipeFormat(); }

        [JsonIgnore]
        public TimeSpan TotalTimespan { get => PrepTimespan + CookTimespan; }

        public string TotalTime { get => PrepTimespan.ToRecipeFormat(); }

        public void AddStep(Step step)
        {
            for (int i = step.Order < 1 ? 1 : step.Order - 1; i < Steps.Count; i++)
                Steps[i - 1].Order = Steps[i - 1].Order + 1;

            Steps.Add(step);
            ReOrderSteps();
        }

        public void AddSteps(IEnumerable<Step> steps)
        {
            Steps.AddRange(steps);
            ReOrderSteps();
        }

        public Recipe WithSteps(IEnumerable<Step> steps)
        {
            AddSteps(steps);
            return this;
        }

        private void ReOrderSteps()
        {
            Steps = Steps.OrderBy(o => o.Order).ToList();
            int index = 1;
            Steps.ForEach(s => s.Order = index++);
        }
    }
}