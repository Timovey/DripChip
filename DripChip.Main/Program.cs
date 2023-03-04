using DripChip.Database;
using DripChip.Database.Implements;
using DripChip.Database.Interfaces;
using DripChip.Database.MappingProfiles;
using DripChip.Main.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.ComponentModel;
using DripChip.DataContracts.JsonHelpers;

var builder = WebApplication.CreateBuilder(args);

// combine models in controller
builder.Services.Configure<ApiBehaviorOptions>(options => 
    options.SuppressInferBindingSourcesForParameters = true);

//culture for dates
//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    options.DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture);
//    options.SupportedCultures = new List<CultureInfo> { CultureInfo.InvariantCulture };
//});

builder.Services.AddDbContext<DripChipContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "DripChip", Version = "v1" });

    swagger.AddSecurityDefinition("Basic", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Basic",
        In = ParameterLocation.Header,
        Description = "Basic scheme",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            new string[] {}
        }
    });
});

//Auth
builder.Services.AddAuthentication("Basic")
                .AddScheme<AuthenticationSchemeOptions,
                    BasicAuthenticationHandler>("Basic", null);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.AllowInputFormatterExceptionMessages = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new NullDateTimeJsonConverter());
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

//!_! ------------------ Mapping Profiles
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());


//!_! ------------------ Cors
builder.Services.AddCors(x => x.AddDefaultPolicy(xx => { xx.AllowAnyOrigin(); xx.AllowAnyHeader(); }));

//!_! ------------------ Life Cycles
builder.Services.AddScoped<IAccountStorage, AccountStorage>();
builder.Services.AddScoped<ILocationStorage, LocationStorage>();
builder.Services.AddScoped<IAnimalStorage, AnimalStorage>();
builder.Services.AddScoped<IAnimalTypeStorage, AnimalTypeStorage>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

var supportedCultures = new[] { new CultureInfo("ru-RU") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseCors();
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DripChipContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
