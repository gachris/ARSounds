namespace ARSounds.EntityFramework.Entities;

public class Audio
{
    public Guid Id { get; set; }

    public string Filename { get; set; } = null!;

    public string AudioType { get; set; } = null!;

    public byte[] AudioBytes { get; set; } = null!;

    public string FileExtension { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public virtual Target Target { get; set; } = null!;
}
