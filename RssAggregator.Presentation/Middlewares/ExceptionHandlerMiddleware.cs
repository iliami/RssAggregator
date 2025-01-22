using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Presentation.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext, 
        ILogger<ExceptionHandlerMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Exception has happened with {RequestPath}, the message is {ExceptionMessage}",
                httpContext.Request.Path.Value, exception.Message);

            ProblemDetails problemDetails;
            switch (exception)
            {
                case ForbiddenException forbiddenException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, forbiddenException);
                    break;
                case NotAuthenticatedException notAuthenticatedException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, notAuthenticatedException);
                    logger.LogInformation("{ExceptionType} exception occured: {Exception}", "Not authenticated", notAuthenticatedException);
                    break;
                case ValidationException validationException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                    logger.LogInformation("{ExceptionType} exception occured: {Exception}", "Validation", validationException);
                    break;
                case FeedNotFoundException feedNotFoundException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, feedNotFoundException);
                    logger.LogError("{ExceptionType} exception occured: {Exception}", "Not found feed", feedNotFoundException);
                    break;
                case PostNotFoundException postNotFoundException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, postNotFoundException);
                    logger.LogError("{ExceptionType} exception occured: {Exception}", "Not found post", postNotFoundException);
                    break;
                case UserNotFoundException userNotFoundException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, userNotFoundException);
                    logger.LogError("{ExceptionType} exception occured: {Exception}", "Not found user", userNotFoundException);
                    break;
                default:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                        httpContext, 
                        StatusCodes.Status500InternalServerError, 
                        "Unhandled error!");
                    logger.LogError("{ExceptionType} exception occured: {Exception}", "Unhandled", exception);
                    break;
            }
            
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
        }
    }
}

public static class ProblemDetailsFactoryExtensions
{
    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory factory, 
        HttpContext httpContext,
        NotAuthenticatedException notAuthenticatedException) =>
        factory.CreateProblemDetails(httpContext,
            StatusCodes.Status401Unauthorized,
            "Not authenticated",
            detail: notAuthenticatedException.Message);

    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory factory, 
        HttpContext httpContext,
        ForbiddenException forbiddenException) =>
        factory.CreateProblemDetails(httpContext,
            StatusCodes.Status403Forbidden,
            "Authorization failed",
            detail: forbiddenException.Message);

    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory factory, 
        HttpContext httpContext,
        INotFoundException notFoundException) =>
        factory.CreateProblemDetails(httpContext, 
            StatusCodes.Status404NotFound,
            notFoundException.Message);

    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory factory, 
        HttpContext httpContext,
        ValidationException validationException)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in validationException.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
        }

        return factory.CreateValidationProblemDetails(httpContext,
            modelStateDictionary,
            StatusCodes.Status400BadRequest,
            "Validation failed");
    }
}