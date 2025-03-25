using ARSounds.Application.ImageRecognition;
using ARSounds.ApplicationFlow;
using ARSounds.Core.App;
using ARSounds.Core.Auth.Events;
using ARSounds.Core.Targets;
using ARSounds.Core.Targets.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application.Targets.Queries;

public class GetTargetsQueryHandler : IRequestHandler<GetTargetsQuery, GetTargetsResultDto>
{
    #region Fields/Consts

    private readonly ITargetsService _targetsService;
    private readonly IApplicationEvents _applicationEvents;
    private readonly ITargetsComponent _targets;

    #endregion

    public GetTargetsQueryHandler(ITargetsService targetsService, IAppRoot appRoot, IApplicationEvents applicationEvents)
    {
        _targetsService = targetsService;
        _targets = appRoot.Targets;
        _applicationEvents = applicationEvents;
    }

    public async Task<GetTargetsResultDto> Handle(GetTargetsQuery request, CancellationToken cancellationToken)
    {
        _applicationEvents.Raise(new TargetsUpdatedStartedEvent());

        var resultDto = await HandleInnerAsync(request, cancellationToken);

        _applicationEvents.Raise(new TargetsUpdatedFinishedEvent());

        return resultDto;
    }

    public async Task<GetTargetsResultDto> HandleInnerAsync(GetTargetsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var responseMessage = await _targetsService.GetAsync(cancellationToken);

            if (responseMessage.StatusCode is ImageRecognition.Response.StatusCode.Success)
            {
                var targets = responseMessage.Response.Result;
                _targets.SetTargetsResult(targets);
            }

            return new GetTargetsResultDto(true);
        }
        catch (Exception ex)
        {
            return new GetTargetsResultDto(false, ex.ToString());
        }
    }
}