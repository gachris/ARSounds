using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ARSounds.Server.Core.Filters;

public class TargetBrowserQuery : BrowserQuery, IBrowserQuery, IPaginationFilter
{
    [FromQuery(Name = "description")]
    [JsonPropertyName("description")]
    public virtual string? Description { get; set; }

    [FromQuery(Name = "created")]
    [JsonPropertyName("created")]
    public virtual DateTime? Created { get; set; }
}
