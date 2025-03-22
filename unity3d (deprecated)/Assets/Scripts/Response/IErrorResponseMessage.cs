using System.Collections.Generic;

namespace Assets
{
    public interface IErrorResponseMessage : IResponseMessage
    {
        List<Error> Errors { get; }
    }
}