namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    public interface IResult
    {
        IResult AddError(string message);
        void SetFailureReason(FailureReason failureReason);
    }
}