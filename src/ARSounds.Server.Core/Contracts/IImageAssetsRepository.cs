using ARSounds.EntityFramework.Entities;

namespace ARSounds.Server.Core.Contracts;

/// <summary>
/// Provides asynchronous methods to manage image asset entities.
/// </summary>
public interface IImageAssetsRepository : IGenericRepository<ImageAsset>
{
}