using OnDemandTutor.Helper.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System.Net;
using OnDemandTutor.DataAccess;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace OnDemandTutor.API.Extensions
{

    public class WebApiValidationAttribute : ActionFilterAttribute
    {
        readonly IServiceProvider _serviceProvider;
        readonly IStringLocalizer<Resource> _localizer;
        readonly ILogger<WebApiValidationAttribute> _logger;

        public WebApiValidationAttribute(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resource> localizer,
            ILogger<WebApiValidationAttribute> logger
            )
        {
            _serviceProvider = serviceProvider;
            _localizer = localizer;
            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation($"{context.HttpContext.Connection.Id} |" +
                $"{context.HttpContext.Connection.RemoteIpAddress} |" +
                $"{context.HttpContext.Connection.LocalIpAddress} |" +
                $" {context.HttpContext.Request.Path} | " +
                $"{context.ActionArguments.SerializeObject()}");
            return base.OnActionExecutionAsync(context, next);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var allErrors = new List<ValidationErrorModel>();
            if (context.ActionDescriptor.Parameters.Count > 0)
            {

                foreach (var arg in context.ActionDescriptor.Parameters)
                {
                    context.ActionArguments.TryGetValue(arg.Name, out object? value);
                    if (arg.ParameterType is null)
                        continue;

                    Type genericType = typeof(IValidator<>).MakeGenericType(arg.ParameterType);
                    var validator = _serviceProvider.GetService(genericType);

                    if (validator == null)
                        continue;

                    if (value != null)
                    {
                        var ValidateFn = validator.GetType()?
                            .GetMethod("Validate", new Type[] { arg.ParameterType });

                        if (ValidateFn != null)
                        {
                            try
                            {
                                var result = (ValidationResult?)ValidateFn.Invoke(validator, new[] { value });

                                if (result != null && !result.IsValid)
                                {
                                    allErrors.AddRange(result.Errors.Select(err => new ValidationErrorModel(err.PropertyName, err.ErrorMessage, err.ErrorCode)).ToList());
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!context.ModelState.IsValid)
                                {
                                    foreach (var item in context.ModelState)
                                    {
                                        if (item.Value.Errors != null)
                                        {
                                            foreach (var error in item.Value.Errors)
                                            {
                                                allErrors.Add(new ValidationErrorModel(item.Key, error.ErrorMessage, error.ErrorMessage));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    allErrors.Add(new ValidationErrorModel("Exception", ex.Message, ex.Message));
                                }
                            }

                        }
                    }
                    else
                    {
                        if (!context.ModelState.IsValid)
                        {
                            foreach (var item in context.ModelState)
                            {
                                if (item.Value.Errors != null)
                                {
                                    foreach (var error in item.Value.Errors)
                                    {
                                        allErrors.Add(new ValidationErrorModel(item.Key, error.ErrorMessage, error.ErrorMessage));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (allErrors.Count > 0)
            {
                var orror = new ApiErrorActionResult()
                {
                    Title = nameof(HttpStatusCode.BadRequest),
                    Status = StatusCodes.Status400BadRequest,
                    Errors = allErrors.DistinctBy(s => new
                    { s.PropertyName, s.ErrorCode }).ToList(),
                };
                context.Result = new BadRequestObjectResult(orror);
            }
            base.OnActionExecuting(context);
        }

    }
}
