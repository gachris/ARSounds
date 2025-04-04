﻿namespace ARSounds.ApiClient.Response;

public interface IResponseMessage
{
    Guid TransactionId { get; }

    StatusCode StatusCode { get; set; }
}

public interface IResponseMessage<TResult> : IResponseMessage
{
    ResponseDoc<TResult> Response { get; }
}