using TrackHub.Manager.Application.GeocodingProviders.Commands.Create;
using TrackHub.Manager.Application.GeocodingProviders.Commands.Delete;
using TrackHub.Manager.Application.GeocodingProviders.Commands.SetActive;
using TrackHub.Manager.Application.GeocodingProviders.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<GeocodingProviderVm> CreateGeocodingProvider([Service] ISender sender, CreateGeocodingProviderCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateGeocodingProvider([Service] ISender sender, Guid id, UpdateGeocodingProviderCommand command)
    {
        if (id != command.Id) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteGeocodingProvider([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteGeocodingProviderCommand(id));
        return id;
    }

    public async Task<bool> SetActiveGeocodingProvider([Service] ISender sender, Guid id)
    {
        await sender.Send(new SetActiveGeocodingProviderCommand(id));
        return true;
    }
}
