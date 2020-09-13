using System;

namespace BadMelon.Data.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string messagePrefix = "Could not find ";

        public EntityNotFoundException(string message) : base(messagePrefix + message)
        {
        }

        public EntityNotFoundException(string message, System.Exception innerException) : base(messagePrefix + message, innerException)
        {
        }
    }
}