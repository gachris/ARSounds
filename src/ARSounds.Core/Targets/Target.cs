namespace ARSounds.Core.Targets;

public record Target(
    Guid Id,
    string Name,
    string Audio,
    bool IsActive,
    string? Image,
    Guid? OpenVisionId,
    bool IsTrackable,
    string? Color,
    int? Rate,
    DateTimeOffset Created,
    DateTimeOffset Updated);
