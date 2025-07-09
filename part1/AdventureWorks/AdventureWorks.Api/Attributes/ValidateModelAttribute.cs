using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Api.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var arg in context.ActionArguments)
        {
            if (arg.Value is IValidatableObject validatableObject)
            {
                var validationContext = new ValidationContext(validatableObject);
                var results = validatableObject.Validate(validationContext).ToList();

                foreach (var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        context.ModelState.AddModelError(memberName, result.ErrorMessage ?? "Invalid");
                    }
                }
            }
        }
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                            .Where(x => x.Value?.Errors?.Any() == true)
                            .SelectMany(x => x.Value?.Errors?.Where(e => e != null)
                            .Select(e => new ValidationFailure(x.Key, e.ErrorMessage)) ?? [])
                            .ToList();

            throw new FluentValidation.ValidationException(errors);
        }
    }
}
