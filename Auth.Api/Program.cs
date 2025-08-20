using System.Text;
using Auth.Api.Middlewares;
using Auth.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IEmailService, EmailService>();
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("https://localhost:5000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GatewayValidationMiddleware>();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();