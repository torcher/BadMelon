namespace BadMelon.Data.Helpers
{
    public class JwtSettings
    {
        private string _Secret = "";

        public string Secret
        {
            get => _Secret;
            set
            {
                if (value != null && _Secret.Length == 0)
                    _Secret = value;
            }
        }

        private int _Expiry = 0;

        public int ExpiryInDays
        {
            get => _Expiry;
            set
            {
                if (value != 0 && _Expiry == 0)
                    _Expiry = value;
            }
        }
    }
}