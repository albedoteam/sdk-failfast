using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlbedoTeam.Sdk.ExceptionHandler.Exceptions;
using AlbedoTeam.Sdk.FailFast.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AlbedoTeam.Sdk.FailFast
{
    public class FailFastRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class, IResponse, new()
    {
        private readonly IEnumerable<IValidator> _validators;

        public FailFastRequestBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
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

        private static Task<TResponse> TryNext(RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                var interResult = next().Result;
                return Task.FromResult(interResult);
            }
            catch (Exception e)
            {
                var errorResponse = new TResponse();

                if (e.InnerException?.GetType() == typeof(ResourceExistsException))
                    errorResponse.SetConflict();
                else if (e.InnerException?.GetType() == typeof(NotFoundException)) // notfound
                    errorResponse.SetNotFound();
                else if (e.InnerException?.GetType() == typeof(BadRequestException)) // bad request
                    errorResponse.AddError(e.InnerException?.Message);
                else
                    errorResponse.AddError("Ooooooooooooops :/ Um baita erro ocorreu!");

                return Task.FromResult(errorResponse);
            }
        }

        private static Task<TResponse> ValidationErrors(IEnumerable<ValidationFailure> failures)
        {
            var errorResponse = new TResponse();

            foreach (var failure in failures) errorResponse.AddError(failure.ErrorMessage);

            return Task.FromResult(errorResponse);
        }
    }
}