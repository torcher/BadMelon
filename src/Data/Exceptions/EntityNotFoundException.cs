namespace BadMelon.Data.Exceptions
{
    public class EntityNotFoundException : RepoException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}