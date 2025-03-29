namespace ARSounds.UI.WinUI.Contracts;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
