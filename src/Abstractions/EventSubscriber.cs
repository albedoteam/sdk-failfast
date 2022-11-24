namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MediatR;

    public abstract class EventSubscriber<TEvent> : INotificationHandler<TEvent>
        where TEvent : INotification
    {
        public async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await Task.FromResult(new Result<TEvent>(FailureReason.RequestCancelled));
            }

            await HandleCommand(notification, cancellationToken);
        }

        protected abstract Task HandleCommand(TEvent @event, CancellationToken cancellationToken);
    }
}