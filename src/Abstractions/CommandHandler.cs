namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public abstract class CommandHandler<TCommand, TData> : IRequestHandler<TCommand, Result<TData>>
        where TCommand : IRequest<Result<TData>>
        where TData : class, new()
    {
        public async Task<Result<TData>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return await Task.FromResult(new Result<TData>(FailureReason.RequestCancelled));
            }

            return await HandleCommand(request, cancellationToken);
        }

        protected abstract Task<Result<TData>> HandleCommand(TCommand request, CancellationToken cancellationToken);
    }
}