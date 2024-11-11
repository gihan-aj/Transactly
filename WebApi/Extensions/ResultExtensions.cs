using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebApi.Extensions
{
    public static class ResultExtensions
    {
        public static ProblemDetails CreateProblemDetails(
            string title,
            int status,
            Error error,
            Error[]? errors = null) =>
            new()
            {
                Title = title,
                Type = error.Code,
                Detail = error.Description,
                Status = status,
                Extensions = { { nameof(errors), errors } }
            };
    }
}
