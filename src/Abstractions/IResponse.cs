namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    public interface IResponse
    {
        IResponse AddError(string message);
        void SetConflict();
    }
}