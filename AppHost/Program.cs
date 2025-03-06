var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiGateway>("api-gateway");
builder.AddProject<Projects.Auth_Api>("auth");
builder.AddProject<Projects.Authors_Api>("authors");
builder.AddProject<Projects.Books_Api>("books");

builder.Build().Run();