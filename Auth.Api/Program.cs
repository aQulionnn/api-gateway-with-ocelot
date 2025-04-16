using System.Text;
using Auth.Api.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAesService, AesService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://localhost:5003";
        options.Audience = "api-gateway";    
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-32-character-long-secret-key!!")),
            ValidateIssuer = true,
            ValidIssuer = "https://localhost:5003",
            ValidateAudience = true,
            ValidAudience = "api-gateway"
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();