using Newtonsoft.Json;
using System;

namespace BadMelon.Data.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UnauthorizedException : Exception
    {
        [JsonProperty]
        public int StatusCode { get => 401; }

        [JsonProperty]
        public string Title { get => "You are not authorized to perform this acton."; }
    }
}