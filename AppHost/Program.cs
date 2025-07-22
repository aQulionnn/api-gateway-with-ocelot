var builder = DistributedApplication.CreateBuilder(args);

var authApi = builder.AddProject<Projects.Auth_Api>("auth");
var authorsApi = builder.AddProject<Projects.Authors_Api>("authors");
var booksApi = builder.AddProject<Projects.Books_Api>("books");

builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithReference(authApi)
    .WithReference(authorsApi)
    .WithReference(booksApi)
    .WithExternalHttpEndpoints();

builder.Build().Run();