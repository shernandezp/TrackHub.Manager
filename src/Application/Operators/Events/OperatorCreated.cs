using Ardalis.GuardClauses;
using Common.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Operators.Events;

public sealed class OperatorCreated
{
    public readonly record struct Notification(CredentialDto Credential) : INotification
    {
        
        public class EventHandler(ICredentialWriter credentialWriter, IConfiguration configuration) : INotificationHandler<Notification>
        {

            public async Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                var key = configuration["AppSettings:EncryptionKey"];
                Guard.Against.Null(key, message: "Credential key not found.");
                var salt = CryptographyExtensions.GenerateAesKey(256);
                await credentialWriter.CreateCredentialAsync(notification.Credential, salt, key, cancellationToken); 
            }
        }
    }
}
