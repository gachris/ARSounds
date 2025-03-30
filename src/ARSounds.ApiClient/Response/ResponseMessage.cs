using Newtonsoft.Json;

namespace ARSounds.ApiClient.Response;

public class ResponseMessage : IResponseMessage
{
    [JsonProperty("transaction_id")]
    public Guid TransactionId { get; }

    [JsonProperty("status_code")]
    public StatusCode StatusCode { get; set; }

    public ResponseMessage()
    {
        TransactionId = Guid.NewGuid();
    }
}

public class ResponseMessage<TResult> : ResponseMessage, IResponseMessage, IResponseMessage<TResult>
{
    [JsonProperty("response")]
    public ResponseDoc<TResult> Response { get; }

    public ResponseMessage(ResponseDoc<TResult> response)
    {
        Response = response;
    }
}
