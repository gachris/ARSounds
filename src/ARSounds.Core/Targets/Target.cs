namespace ARSounds.Core.Targets;

public record Target(
    Guid Id,
    string Description,
    string Title,
    string AudioType,
    string AudioBase64,
    string? ImageBase64,
    Guid? VisionTargetId,
    bool IsActive,
    bool IsTrackable,
    string? HexColor,
    int? Rate,
    DateTime Created,
    DateTime Updated);
