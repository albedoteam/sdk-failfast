namespace AlbedoTeam.Sdk.FailFast.Abstractions
{
    // public interface IResponse
    // {
    //     IResponse AddError(string message);
    //     void SetConflict();
    //     void SetNotFound();
    // }

    public interface IResult
    {
        IResult AddError(string message);
    }
}