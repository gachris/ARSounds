using Newtonsoft.Json;
using System.Collections.Generic;

namespace ARSounds.Application.ImageRecognition.Response
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
