using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ARSounds.Server.Core.Requests;

/// <summary>
/// Represents a browser query for filtering targets with additional parameters.
/// </summary>
public class TargetBrowserQuery : BrowserQuery, IBrowserQuery
{
    /// <summary>
    /// Gets or sets an optional description filter for the targets.
    /// </summary>
    [FromQuery(Name = "description")]
    [JsonPropertyName("description")]
    public virtual string? Description { get; set; }
}