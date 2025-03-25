using ARSounds.Application.Targets.Queries;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Targets.Events;
using ARSounds.UI.Camera.ViewModels;
using ARSounds.UI.Extensions;
using ARSounds.UI.Services;
using ARSounds.UI.Targets.ViewModels;
using ARSounds.UI.User.ViewModels;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.Maui.Networking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ARSounds.UI.Common.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly ObservableCollection<TargetViewModel> _targetsCollection = new();
    private readonly IReadOnlyCollection<TargetViewModel> _readOnlyTargetsCollection;
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IConnectivity _connectivity;

    #endregion

    #region Properties

    public IReadOnlyCollection<TargetViewModel> Targets => _readOnlyTargetsCollection;

    #endregion

    public HomeViewModel(IMediator mediator, INavigationService navigationService, IConnectivity connectivity, IApplicationEvents applicationEvents) : base(navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _connectivity = connectivity;
        _readOnlyTargetsCollection = new ReadOnlyObservableCollection<TargetViewModel>(_targetsCollection);

        applicationEvents.Register<TargetsUpdatedStartedEvent>(OnTargetsUpdatedStarted);
        applicationEvents.Register<TargetsUpdatedFinishedEvent>(OnTargetsUpdatedFinished);
        applicationEvents.Register<TargetsUpdatedEvent>(OnTargetsUpdated);
    }

    #region Methods

    private void OnTargetsUpdatedStarted(TargetsUpdatedStartedEvent obj)
    {
        SetDataLoadingIndicators(true);
        LoadingText = "Loading...";
    }

    private void OnTargetsUpdatedFinished(TargetsUpdatedFinishedEvent obj)
    {
        SetDataLoadingIndicators(false);
    }

    private void OnTargetsUpdated(TargetsUpdatedEvent obj)
    {
        _targetsCollection.Clear();
        _targetsCollection.AddRange(obj.Items.Select(TargetViewModel.FromTarget).Take(4));
    }

    public override async Task InitializeAsync(object navigationData)
    {
        await _mediator.Send(new GetTargetsQuery());
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task OpenCamera()
    {
        await _navigationService.PushAsync(typeof(CameraViewModel));
    }

    [RelayCommand]
    private async Task OpenUserProfile()
    {
        await _navigationService.PushAsync(typeof(ProfileViewModel));
    }

    [RelayCommand]
    private async Task ViewAllTarges()
    {
        await _navigationService.PushAsync(typeof(TargetsListViewModel));
    }

    [RelayCommand]
    private async Task OpenDetails()
    {
        await _navigationService.PushAsync(typeof(TargetDetailsViewModel));
    }

    #endregion
}
