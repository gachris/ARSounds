using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets
{
    public class ErrorResponseMessage : ResponseMessage, IErrorResponseMessage
    {
        [JsonProperty("errors")]
        public List<Error> Errors { get; }

        public ErrorResponseMessage()
        {
            Errors = new List<Error>();
        }
    }
}
