using System.Collections.Generic;

namespace ARSounds.Application.Response
{
    public interface IErrorResponseMessage : IResponseMessage
    {
        List<Error> Errors { get; }
    }
}