using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    public abstract class QueryHandler<TQuery, TData> : IRequestHandler<TQuery, Result<TData>>
        where TQuery : IRequest<Result<TData>>
        where TData : class, new()
    {
        public async Task<Result<TData>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            return await Handle(request);
        }

        protected abstract Task<Result<TData>> Handle(TQuery request);
    }
}