using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrganizationApi.Accessor;
using OrganizationApi.Entities;
using OrganizationApi.Manager;
using OrganizationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<HotelManager>();
builder.Services.AddSingleton<RoomManager>();
builder.Services.AddSingleton<MongoService>();
builder.Services.AddHostedService<IsEmptyRoomsService>();
builder.Services.AddScoped<UserAcessor>();


builder.Services.AddHttpContextAccessor();

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
//builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseCors(cors=>
{
    cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
