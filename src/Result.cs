namespace AlbedoTeam.Sdk.FailFast
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Abstractions;

    public class Result<TData> : IResult
    {
        private readonly IList<string> _messages = new List<string>();

        public Result()
        {
            FailureReason = FailFast.FailureReason.InternalServerError;
            Errors = new ReadOnlyCollection<string>(_messages);
        }

        public Result(FailureReason failureReason, string errorMessage = null)
        {
            FailureReason = failureReason;

            _messages.Add(string.IsNullOrWhiteSpace(errorMessage)
                ? failureReason.ToString()
                : $"{failureReason.ToString()} - {errorMessage}");

            Errors = new ReadOnlyCollection<string>(_messages);
        }

        public Result(TData data)
        {
            Data = data;
            Errors = new ReadOnlyCollection<string>(_messages);
        }

        public IEnumerable<string> Errors { get; set; }

        public TData Data { get; set; }

        public bool HasError => Errors.Any();

        public FailureReason? FailureReason { get; private set; }

        public IResult AddError(string message)
        {
            _messages.Add(message);
            return this;
        }

        public void SetFailureReason(FailureReason failureReason)
        {
            FailureReason = failureReason;
        }
    }
}