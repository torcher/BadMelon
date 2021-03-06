﻿using System;

namespace BadMelon.Exceptions
{
    public class IngredientMismatchException : Exception
    {
        public IngredientMismatchException(string message) : base(message)
        {
        }

        public IngredientMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}