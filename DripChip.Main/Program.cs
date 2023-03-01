using DripChip.Database;
using DripChip.Database.Implements;
using DripChip.Database.Interfaces;
using DripChip.Database.MappingProfiles;
using DripChip.Main.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


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

builder.Services.AddControllers();

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

if (app.Environment.IsDevelopment() || 1 == 1)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


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
