namespace ARSounds.Web.Api.EntityFramework.Entities;

public class Audio
{
    public Guid Id { get; set; }

    public string Filename { get; set; }

    public string AudioType { get; set; }

    public byte[] AudioBytes { get; set; }

    public string FileExtension { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public virtual Target Target { get; set; }
}
