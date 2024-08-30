using Common.Application.Interfaces;
using Common.Domain.Constants;
using Common.Infrastructure;
using GraphQL;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Infrastructure.SecurityApi;

public class SecurityWriter(IGraphQLClientFactory graphQLClient)
    : GraphQLService(graphQLClient.CreateClient(Clients.Security)), ISecurityWriter
{
    // Creates a new user asynchronously.
    public async Task<UserVm> CreateUserAsync(CreateUserDto user, CancellationToken token)
    {
        var request = new GraphQLRequest
        {
            Query = @"
                    mutation($username: String!, $password: String!, $lastName: String!, $firstName: String!, $emailAddress: String!, $active: Boolean!, $accountId: UUID!)
                    {
                      createManager(
                        command: {
                          user: {
                            username: $username
                            password: $password
                            lastName: $lastName
                            firstName: $firstName
                            emailAddress: $emailAddress
                            active: $active
                          }
                          accountId: $accountId
                        }
                      ) {
                            userId
                            accountId
                            active
                            username
                        }
                    }",
            Variables = new
            {
                user.Username,
                user.Password,
                user.LastName,
                user.FirstName,
                user.EmailAddress,
                user.Active,
                user.AccountId
            }
        };
        var result = await MutationAsync<UserVm>(request, token);
        return result;
    }
}
