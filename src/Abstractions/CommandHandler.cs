using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    public abstract class CommandHandler<TCommand, TData> : IRequestHandler<TCommand, Result<TData>>
        where TCommand : IRequest<Result<TData>>
        where TData : class, new()
    {
        public async Task<Result<TData>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            return await Handle(request);
        }

        protected abstract Task<Result<TData>> Handle(TCommand request);
    }
}