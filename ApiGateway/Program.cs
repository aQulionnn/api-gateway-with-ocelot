using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var ocelotConfigPath = Path.Combine(builder.Environment.ContentRootPath, "OcelotConfig");
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddOcelot(ocelotConfigPath, builder.Environment);

builder.Services.AddOcelot(builder.Configuration);

builder.Services
    .AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://localhost:5003";
        options.Audience = "api-gateway";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-32-character-long-secret-key!!")),
            ValidateIssuer = true,
            ValidIssuer = "https://localhost:5003",
            ValidateAudience = true,
            ValidAudience = "api-gateway"
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseOcelot();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();