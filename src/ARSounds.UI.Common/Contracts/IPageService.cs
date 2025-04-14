namespace ARSounds.UI.Common.Contracts;

public interface IPageService
{
    Type GetPageType(string key);

    void Configure(string key, Type type);
}
