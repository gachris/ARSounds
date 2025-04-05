using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Requests;
using Microsoft.AspNetCore.WebUtilities;

namespace ARSounds.Server.Core.Services;

/// <summary>
/// Service for creating URIs for pagination and details endpoints.
/// </summary>
public class UriService : IUriService
{
    #region Fields/Consts

    private readonly string _baseUri;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="UriService"/> class.
    /// </summary>
    /// <param name="baseUri">The base URI for the service.</param>
    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    #region IUriService Implementation

    /// <inheritdoc/>
    public Uri GetPageUri(BrowserQuery filter, string route)
    {
        var endpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "page", filter.Page.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "size", filter.Size.ToString());
        return new Uri(modifiedUri);
    }

    #endregion
}