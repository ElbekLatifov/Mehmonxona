using ChatApi.Acessor;
using ChatApi.Context;
using ChatApi.Errors;
using ChatApi.Manager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<ChatManager>();
builder.Services.AddScoped<UserAcessor>();
builder.Services.AddHttpContextAccessor();
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
    var organizationDb = app.Services.GetRequiredService<AppDbContext>();
    organizationDb.Database.Migrate();
}
app.UseCors(c =>
{
    c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});
app.UseLoggerMiddleware();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LoggerMiddleware>();
app.MapControllers();

app.Run();
