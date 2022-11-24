namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public abstract class QueryHandler<TQuery, TData> : IRequestHandler<TQuery, Result<TData>>
        where TQuery : IRequest<Result<TData>>
        where TData : class, new()
    {
        public async Task<Result<TData>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return await Task.FromResult(new Result<TData>(FailureReason.RequestCancelled));
            }

            return await HandleCommand(request, cancellationToken);
        }

        protected abstract Task<Result<TData>> HandleCommand(TQuery request, CancellationToken cancellationToken);
    }
}