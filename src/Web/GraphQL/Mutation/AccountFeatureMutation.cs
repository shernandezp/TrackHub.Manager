using TrackHub.Manager.Application.AccountFeatures.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<AccountFeatureVm> SetAccountFeature([Service] ISender sender, SetAccountFeatureCommand command) => await sender.Send(command);
    public async Task<AccountFeatureVm> SetAccountFeatureMaster([Service] ISender sender, SetAccountFeatureMasterCommand command) => await sender.Send(command);
    public async Task<bool> DisableAccountFeature([Service] ISender sender, DisableAccountFeatureCommand command) { await sender.Send(command); return true; }
    public async Task<bool> UpdateAccountFeatureConfiguration([Service] ISender sender, UpdateAccountFeatureConfigurationCommand command) { await sender.Send(command); return true; }
    public async Task<int> SeedPublicLinksFeature([Service] ISender sender) => await sender.Send(new SeedPublicLinksFeatureCommand());
}
