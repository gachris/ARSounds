using ARSounds.Application.Dtos;
using ARSounds.Application.Response;
using ARSounds.Application.Services;
using ARSounds.ApplicationFlow;
using ARSounds.Core.App;
using ARSounds.Core.Targets;
using ARSounds.Core.Targets.Events;
using MediatR;

namespace ARSounds.Application.Queries;

public class GetTargetsQueryHandler : IRequestHandler<GetTargetsQuery, RequestResultDto>
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

    public async Task<RequestResultDto> Handle(GetTargetsQuery request, CancellationToken cancellationToken)
    {
        _applicationEvents.Raise(new TargetsUpdatedStartedEvent());

        var resultDto = await HandleInnerAsync(request, cancellationToken);

        _applicationEvents.Raise(new TargetsUpdatedFinishedEvent());

        return resultDto;
    }

    public async Task<RequestResultDto> HandleInnerAsync(GetTargetsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var responseMessage = await _targetsService.GetAsync(cancellationToken);

            if (responseMessage.StatusCode is StatusCode.Success)
            {
                var targets = responseMessage.Response.Result;
                _targets.SetTargetsResult(targets);
            }

            return new RequestResultDto();
        }
        catch (Exception ex)
        {
            return new RequestResultDto(ex.ToString(), ex);
        }
    }
}