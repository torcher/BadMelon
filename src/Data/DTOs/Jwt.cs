namespace BadMelon.Data.DTOs
{
    public class Jwt
    {
        private readonly string _token;

        public Jwt(string token)
        {
            _token = token;
        }

        public string token
        {
            get => _token;
        }
    }
}