using IdentityApi.Context;
using IdentityApi.Errors;
using IdentityApi.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>();
builder.Services.Configure<JwtDto>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    var signingKey = System.Text.Encoding.UTF32.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SigningKey")!);

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:ValidIssuer"),
        ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:ValidAudience"),
        ValidateIssuer = true,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(signingKey),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var logger = new LoggerConfiguration().WriteTo.File("logger.txt",
        LogEventLevel.Information,
        rollingInterval: RollingInterval.Day).CreateLogger();

builder.Services.AddSerilog(logger);
builder.Services.AddScoped<GetToken>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Bearer. : \"Authorization: Bearer { token }\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});



var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

if (app.Services.GetService<AppDbContext>() != null)
{
    var chatDb = app.Services.GetRequiredService<AppDbContext>();
    chatDb.Database.Migrate();
}


app.UseHttpsRedirection();

app.UseCors(cors=>
{
    cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggerMiddleware>();

app.MapControllers();

app.Run();
