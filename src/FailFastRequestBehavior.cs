using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlbedoTeam.Sdk.FailFast.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AlbedoTeam.Sdk.FailFast
{
    public class FailFastRequestBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : class, IResult, new()
    {
        private readonly IEnumerable<IValidator> _validators;

        public FailFastRequestBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResult> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResult> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            return failures.Any()
                ? ValidationErrors(failures)
                : TryNext(next);
        }

        private static Task<TResult> TryNext(RequestHandlerDelegate<TResult> next)
        {
            try
            {
                var interResult = next().Result;
                return Task.FromResult(interResult);
            }
            catch
            {
                var errorResponse = new TResult();
                errorResponse.SetFailureReason(FailureReason.InternalServerError);
                errorResponse.AddError("Ooops! Um baita erro ocorreu, corra para as montanhas!!");

                return Task.FromResult(errorResponse);
            }
        }

        private static Task<TResult> ValidationErrors(IEnumerable<ValidationFailure> failures)
        {
            var errorResponse = new TResult();
            errorResponse.SetFailureReason(FailureReason.BadRequest);
            foreach (var failure in failures)
                errorResponse.AddError(failure.ErrorMessage);

            return Task.FromResult(errorResponse);
        }
    }
}