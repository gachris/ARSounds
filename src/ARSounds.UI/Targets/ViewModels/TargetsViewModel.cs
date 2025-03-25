using ARSounds.Application.Targets.Queries;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Targets.Events;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Extensions;
using ARSounds.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.Maui.Networking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ARSounds.UI.Targets.ViewModels;

public partial class TargetsListViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly ObservableCollection<TargetViewModel> _targetsCollection = new();
    private readonly IReadOnlyCollection<TargetViewModel> _readOnlyTargetsCollection;
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IConnectivity _connectivity;

    [ObservableProperty]
    public bool _isRefreshing;

    #endregion

    #region Properties

    public IReadOnlyCollection<TargetViewModel> Targets => _readOnlyTargetsCollection;

    #endregion

    public TargetsListViewModel(IMediator mediator, INavigationService navigationService, IConnectivity connectivity, IApplicationEvents applicationEvents) : base(navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _connectivity = connectivity;
        _readOnlyTargetsCollection = new ReadOnlyObservableCollection<TargetViewModel>(_targetsCollection);

        applicationEvents.Register<TargetsUpdatedEvent>(OnTargetsUpdated);
        applicationEvents.Register<TargetsUpdatedStartedEvent>(OnTargetsUpdatedStarted);
        applicationEvents.Register<TargetsUpdatedFinishedEvent>(OnTargetsUpdatedFinished);
    }

    #region Methods

    private void OnTargetsUpdatedStarted(TargetsUpdatedStartedEvent obj)
    {
        IsRefreshing = true;
    }

    private void OnTargetsUpdatedFinished(TargetsUpdatedFinishedEvent obj)
    {
        IsRefreshing = false;
    }

    private void OnTargetsUpdated(TargetsUpdatedEvent obj)
    {
        _targetsCollection.Clear();
        _targetsCollection.AddRange(obj.Items.Select(TargetViewModel.FromTarget));
    }

    public override async Task InitializeAsync(object navigationData)
    {
        await _mediator.Send(new GetTargetsQuery());
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task Refresh()
    {
        await _mediator.Send(new GetTargetsQuery());
    }

    [RelayCommand]
    private async Task ShowDetails(TargetViewModel obj)
    {
        await _navigationService.PushModalAsync(typeof(TargetDetailsViewModel));
    }

    #endregion
}
