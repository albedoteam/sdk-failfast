using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, Response<TResult>>
        where TCommand : IRequest<Response<TResult>>
        where TResult : class, new()
    {
        public async Task<Response<TResult>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            return new Response<TResult>(await Handle(request));
        }

        protected abstract Task<TResult> Handle(TCommand request);
    }
}