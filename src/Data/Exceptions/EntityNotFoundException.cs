using BadMelon.Data.Entities;
using System;

namespace BadMelon.Data.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string messagePrefix = "Could not find ";

        public EntityNotFoundException(Type t) : base(messagePrefix + TranslateTypeToString(t))
        {
        }

        public EntityNotFoundException(string message) : base(messagePrefix + message)
        {
        }

        public EntityNotFoundException(string message, System.Exception innerException) : base(messagePrefix + message, innerException)
        {
        }

        private static string TranslateTypeToString(Type t)
        {
            if (t == typeof(IngredientType))
                return "ingredient type";
            return "entity";
        }
    }
}