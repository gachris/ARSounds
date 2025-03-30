using ARSounds.Application.Commands;
using ARSounds.UI.Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using MediatR;

namespace ARSounds.UI.Common.ViewModels;

public class ShellViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    private readonly IMediator _mediator;

    private bool _isSettingsOpen;
    private string _selectedViewItem = PageKeys.CameraPage;

    #endregion

    #region Properties

    public bool IsSettingsOpen
    {
        get => _isSettingsOpen;
        protected set => SetProperty(ref _isSettingsOpen, value);
    }

    public string SelectedViewItem
    {
        get => _selectedViewItem;
        set
        {
            SetProperty(ref _selectedViewItem, value);
            OnSelectedViewItemChanged();
        }
    }

    #endregion

    public ShellViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Methods

    public virtual async Task InitializeAsync()
    {
        await _mediator.Send(new SignInSilentCommand());
    }

    public void OnNavigated()
    {
    }

    public void OnNavigatedAway()
    {
    }

    protected virtual void OnSelectedViewItemChanged()
    {
    }

    #endregion
}
