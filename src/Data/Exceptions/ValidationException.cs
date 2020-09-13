using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BadMelon.Data.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ValidationException : Exception
    {
        [JsonProperty]
        public int StatusCode { get => 400; }

        [JsonProperty]
        public string Title { get => "One or more validation errors occured."; }

        [JsonProperty]
        public Dictionary<string, string> Errors { get; set; }

        public ValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}