using AdventureWorks.Api.AutoMappers;
using AdventureWorks.Api.Extensions;
using AdventureWorks.Repository.AutoMappers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddServices(builder.Configuration)
                .AddAutoMapper(typeof(ModelMapperProfileBase),
                               typeof(EntityMapperProfileBase))
                .AddFluentValidation(typeof(Program).Assembly)
                .AddControllers();

builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen();

var app = builder.Build();
app.UseCustomExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
