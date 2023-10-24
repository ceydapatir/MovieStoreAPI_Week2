

using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieStoreAPI.Base.Logger;
using MovieStoreAPI.Base.Middleware;
using MovieStoreAPI.Base.Services;
using MovieStoreAPI.Base.Token;
using MovieStoreAPI.Data.Context;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager Configuration = builder.Configuration; 

var JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
builder.Services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie Store Api Management", Version = "v1.0" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Movie Store Management",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});


builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters{
                ValidateIssuer = true,
                ValidIssuer = JwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Secret)),
                ValidAudience = JwtConfig.Audience,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        }
        );


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context Service
builder.Services.AddDbContext<MovieStoreDBContext>();

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Logger Services
builder.Services.AddSingleton<ILoggerService,ConsoleLogger>();
builder.Services.AddSingleton<ILoggerService,DBLogger>();

// Automapper Service
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<HeartBeatMiddleware>();
Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
{
    Log.Information("-------------Request-Begin------------");
    Log.Information(requestProfilerModel.Request);
    Log.Information(Environment.NewLine);
    Log.Information(requestProfilerModel.Response);
    Log.Information("-------------Request-End------------");
};
app.UseMiddleware<RequestLoggingMiddleware>(requestResponseHandler);

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();
