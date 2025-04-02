namespace ARSounds.Server.EntityFramework.Entities;

public partial class Target
{
    public Guid Id { get; set; }

    public required string Description { get; set; }

    public required string UserId { get; set; }

    public Guid AudioId { get; set; }

    public Guid? ImageId { get; set; }

    public string? HexColor { get; set; }

    public string? Metadata { get; set; }

    public bool IsActive { get; set; }

    public bool IsTrackable { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public virtual Audio Audio { get; set; } = null!;

    public virtual Image? Image { get; set; }
}
