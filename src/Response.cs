using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AlbedoTeam.Sdk.FailFast.Abstractions;

namespace AlbedoTeam.Sdk.FailFast
{
    // public class Response<TResult> : IResponse
    // {
    //     private readonly IList<string> _messages = new List<string>();
    //
    //     public Response()
    //     {
    //         Errors = new ReadOnlyCollection<string>(_messages);
    //     }
    //
    //     public Response(TResult result)
    //     {
    //         Result = result;
    //         Errors = new ReadOnlyCollection<string>(_messages);
    //     }
    //
    //     public IEnumerable<string> Errors { get; }
    //
    //     public TResult Result { get; }
    //
    //     public bool HasError => Errors.Any();
    //
    //     public bool NotFound { get; private set; }
    //
    //     public bool Conflict { get; private set; }
    //
    //     public IResponse AddError(string message)
    //     {
    //         _messages.Add(message);
    //         return this;
    //     }
    //
    //     public void SetConflict()
    //     {
    //         Conflict = true;
    //     }
    //
    //     public void SetNotFound()
    //     {
    //         NotFound = true;
    //     }
    // }

    public class Response<TResult> : IResponse
    {
        private readonly IList<string> _messages = new List<string>();

        public Response(ResponseError responseError, string errorMessage = null)
        {
            ResponseResponseError = responseError;

            _messages.Add(string.IsNullOrWhiteSpace(errorMessage)
                ? responseError.ToString()
                : $"{responseError.ToString()} - {errorMessage}");

            Errors = new ReadOnlyCollection<string>(_messages);
        }

        public Response(TResult result)
        {
            Result = result;
            ResponseResponseError = null;
            Errors = new ReadOnlyCollection<string>(_messages);
        }

        public IEnumerable<string> Errors { get; }

        public TResult Result { get; }

        public bool HasError => Errors.Any();

        public ResponseError? ResponseResponseError { get; }

        public IResponse AddError(string message)
        {
            return this;
        }
    }
}