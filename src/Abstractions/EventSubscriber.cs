namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public abstract class EventSubscriber<TEvent> : INotificationHandler<TEvent>
        where TEvent : INotification
    {
        public async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            await Handle(notification);
        }

        protected abstract Task Handle(TEvent @event);
    }
}