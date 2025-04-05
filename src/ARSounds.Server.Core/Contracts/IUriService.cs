using ARSounds.Server.Core.Requests;

namespace ARSounds.Server.Core.Contracts;

/// <summary>
/// Interface for a service that provides URI generation for pagination and details endpoints.
/// </summary>
public interface IUriService
{
    /// <summary>
    /// Generates a URI for pagination based on the given filter and route.
    /// </summary>
    /// <param name="filter">The pagination filter containing page number and page size.</param>
    /// <param name="route">The route to append to the base URI.</param>
    /// <returns>A URI for the specified pagination parameters.</returns>
    public Uri GetPageUri(BrowserQuery filter, string route);
}