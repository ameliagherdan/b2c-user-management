using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using UserManagementSystem.Application.Interfaces;

namespace UserManagementSystem.Infrastructure.Services;

public class GraphUserService : IGraphUserService
{
    private readonly IConfiguration _config;
    private readonly GraphServiceClient _graphClient;

    public GraphUserService(IConfiguration config)
    {
        _config = config;

        var tenantId = _config["AzureAdB2CGraph:TenantId"];
        var clientId = _config["AzureAdB2CGraph:ClientId"];
        var clientSecret = _config["AzureAdB2CGraph:ClientSecret"];

        var credential = new ClientSecretCredential(
            tenantId,
            clientId,
            clientSecret
        );

        _graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });
    }

    public async Task<string> CreateUserAsync(string email, string password, string displayName)
    {
        var user = new User
        {
            AccountEnabled = true,
            DisplayName = displayName,
            MailNickname = email.Split('@')[0],
            UserPrincipalName = email,
            PasswordProfile = new PasswordProfile
            {
                ForceChangePasswordNextSignIn = false,
                Password = password
            },
            Identities = new List<ObjectIdentity>
            {
                new ObjectIdentity
                {
                    SignInType = "emailAddress",
                    Issuer = $"{_config["AzureAdB2CGraph:TenantId"]}",
                    IssuerAssignedId = email
                }
            }
        };

        var createdUser = await _graphClient.Users.PostAsync(user);
        return createdUser.Id;
    }
}