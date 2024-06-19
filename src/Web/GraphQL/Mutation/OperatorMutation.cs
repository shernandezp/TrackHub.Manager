using TrackHub.Manager.Application.Operators.Commands.Create;
using TrackHub.Manager.Application.Operators.Commands.Delete;
using TrackHub.Manager.Application.Operators.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<OperatorVm> CreateOperator([Service] ISender sender, CreateOperatorCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateOperator([Service] ISender sender, Guid id, UpdateOperatorCommand command)
    {
        if (id != command.Operator.OperatorId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteOperator([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteOperatorCommand(id));
        return id;
    }
}
