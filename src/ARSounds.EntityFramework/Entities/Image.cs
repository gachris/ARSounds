namespace ARSounds.EntityFramework.Entities;

public class Image
{
    public Guid Id { get; set; }

    public Guid? VisionTargetId { get; set; }

    public int? Rate { get; set; }

    public byte[] Buffer { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public virtual Target Target { get; set; } = null!;
}
