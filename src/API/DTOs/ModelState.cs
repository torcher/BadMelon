using System;
using System.Collections.Generic;
using System.Linq;

namespace BadMelon.API.DTOs
{
    public class ModelState
    {
        public string[] Errors { get; }

        public ModelState()
        {
            Errors = Array.Empty<string>();
        }

        public ModelState(IEnumerable<string> errors)
        {
            Errors = errors.ToArray();
        }
    }
}