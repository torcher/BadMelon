namespace BadMelon.Data.DTOs
{
    public class LoginResponse
    {
        public Jwt jwt { get; }
        public bool IsSuccess { get; }

        public LoginResponse()
        {
            IsSuccess = false;
            jwt = null;
        }

        public LoginResponse(Jwt jwt)
        {
            IsSuccess = true;
            this.jwt = jwt;
        }

        public LoginResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
            jwt = null;
        }
    }
}