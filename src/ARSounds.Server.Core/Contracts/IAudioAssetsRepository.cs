using ARSounds.EntityFramework.Entities;

namespace ARSounds.Server.Core.Contracts;

/// <summary>
/// Provides asynchronous methods to access and manipulate audio asset data.
/// </summary>
public interface IAudioAssetsRepository : IGenericRepository<AudioAsset>
{
}