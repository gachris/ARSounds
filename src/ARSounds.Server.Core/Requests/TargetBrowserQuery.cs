using Microsoft.AspNetCore.Mvc;

namespace ARSounds.Server.Core.Requests;

/// <summary>
/// Represents a browser query for filtering targets with additional parameters.
/// </summary>
public class TargetBrowserQuery : BrowserQuery, IBrowserQuery
{
    /// <summary>
    /// Gets or sets an optional name filter for the targets.
    /// </summary>
    [FromQuery(Name = "name")]
    public virtual string? Name { get; set; }
}