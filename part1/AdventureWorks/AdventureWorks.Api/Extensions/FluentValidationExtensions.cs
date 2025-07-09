using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace AdventureWorks.Api.Extensions;

public static class FluentValidationExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly assembly)
    {
        services
            .AddFluentValidationAutoValidation(options =>
            {
                options.DisableDataAnnotationsValidation = true;
            })
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(assembly);
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Continue;
        return services;
    }
}