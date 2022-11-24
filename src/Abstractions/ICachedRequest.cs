namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    using MediatR;

    public interface ICachedRequest<out TResponse> : IRequest<TResponse>
    {
        bool NoCache { get; set; }
    }
}