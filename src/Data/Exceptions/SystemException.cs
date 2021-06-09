using Newtonsoft.Json;
using System;

namespace BadMelon.Data.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SystemException : Exception
    {
        [JsonProperty]
        public int StatusCode { get => 500; }

        [JsonProperty]
        public string Title { get => "A server error has occured. Please contact an administrator"; }

        public SystemException()
        {
        }

        public SystemException(string errorMessage) : base(errorMessage)
        {
        }
    }
}