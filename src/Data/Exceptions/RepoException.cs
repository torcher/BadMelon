using System;

namespace BadMelon.Data.Exceptions
{
    public class RepoException : Exception
    {
        public RepoException(string message) : base(message)
        {
        }

        public RepoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}