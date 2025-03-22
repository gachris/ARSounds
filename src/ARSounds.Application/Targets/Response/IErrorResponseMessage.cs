using System.Collections.Generic;

namespace ARSounds.Application.ImageRecognition.Response
{
    public interface IErrorResponseMessage : IResponseMessage
    {
        List<Error> Errors { get; }
    }
}