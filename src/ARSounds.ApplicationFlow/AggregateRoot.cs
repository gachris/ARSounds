namespace ARSounds.ApplicationFlow;

public abstract class AggregateRoot
{
    #region Fields/Consts

    private readonly List<ApplicationEvent> _applicationEvents = new List<ApplicationEvent>();

    #endregion

    #region Properties

    protected virtual IEnumerable<AggregateRoot> ChildAggregates => new List<AggregateRoot>();

    protected object Locker { get; } = new object();

    #endregion

    #region Methods

    public void ClearEvents()
    {
        lock (Locker)
        {
            _applicationEvents.Clear();

            foreach (var childAggregate in ChildAggregates)
            {
                childAggregate.ClearEvents();
            }
        }
    }

    public virtual IReadOnlyList<ApplicationEvent> TakeApplicationEvents()
    {
        lock (Locker)
        {
            var applicationEvents = _applicationEvents.ToList();
            _applicationEvents.Clear();

            foreach (var childAggregate in ChildAggregates)
            {
                applicationEvents.AddRange(childAggregate.TakeApplicationEvents());
            }

            return applicationEvents.AsReadOnly();
        }
    }

    protected void AddEvent(ApplicationEvent applicationEvent)
    {
        lock (Locker)
        {
            _applicationEvents.Add(applicationEvent);
        }
    }

    #endregion
}