using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using DesafioStone.Interfaces.Services;
using DesafioStone.Services;
using DesafioStone.Repositories;
using DesafioStone.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using DesafioStone.Interfaces.Providers;
using DesafioStone.Providers;
using DesafioStone.Interfaces.Factories;
using DesafioStone.Factories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

//Using custom token validation
/*var key = Encoding.UTF8.GetBytes(new SecretProvider(new ConfigurationBuilder()).Secret());
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});*/

builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, AttributeInjectionProvider>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DesafioStone", Version = "v1" });
    c.DescribeAllParametersInCamelCase();
});

// Dependency Injection
builder.Services.AddTransient<IInvoiceService, InvoiceService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IHashService, HashService>();
builder.Services.AddTransient<IPasswordService, PasswordService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IDbConnectionFactory, MySqlConnectionFactory>();
builder.Services.AddTransient<IConnectionStringProvider, ConnectionStringProvider>();
builder.Services.AddTransient<ISecretProvider, SecretProvider>();
builder.Services.AddTransient<IConfigurationBuilder, ConfigurationBuilder>();
builder.Services.AddTransient<JwtSecurityTokenHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
