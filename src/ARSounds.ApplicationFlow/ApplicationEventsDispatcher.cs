namespace ARSounds.ApplicationFlow;

public class ApplicationEventsDispatcher : IApplicationEventsDispatcher
{
    #region Fields/Consts

    private readonly IApplicationEvents _applicationEvents;
    private readonly IDomainRoot _root;

    #endregion

    public ApplicationEventsDispatcher(IDomainRoot root, IApplicationEvents applicationEvents)
    {
        _root = root;
        _applicationEvents = applicationEvents;
    }

    #region IApplicationEventsDispatcher Implementation

    public void Dispatch()
    {
        IReadOnlyList<ApplicationEvent> events = _root.TakeApplicationEvents().OrderBy(ev => ev.Index).ToList();

        foreach (var applicationEvent in events)
        {
            _applicationEvents.Raise(applicationEvent);
        }
    }

    #endregion
}