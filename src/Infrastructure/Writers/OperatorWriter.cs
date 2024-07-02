using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class OperatorWriter(IApplicationDbContext context) : IOperatorWriter
{
    public async Task<OperatorVm> CreateOperatorAsync(OperatorDto operatorDto, CancellationToken cancellationToken)
    {
        var @operator = new Operator(
            operatorDto.Name,
            operatorDto.Description,
            operatorDto.PhoneNumber,
            operatorDto.EmailAddress,
            operatorDto.Address,
            operatorDto.ContactName,
            (short)operatorDto.ProtocolType,
            operatorDto.AccountId);

        await context.Operators.AddAsync(@operator, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new OperatorVm(
            @operator.OperatorId,
            @operator.Name,
            @operator.Description,
            @operator.PhoneNumber,
            @operator.EmailAddress,
            @operator.Address,
            @operator.ContactName,
            (ProtocolType)@operator.ProtocolType,
            null);
    }

    public async Task UpdateOperatorAsync(UpdateOperatorDto operatorDto, CancellationToken cancellationToken)
    {
        var @operator = await context.Operators.FindAsync([operatorDto.OperatorId], cancellationToken)
            ?? throw new NotFoundException(nameof(Operator), $"{operatorDto.OperatorId}");

        @operator.Name = operatorDto.Name;
        @operator.Description = operatorDto.Description;
        @operator.PhoneNumber = operatorDto.PhoneNumber;
        @operator.EmailAddress = operatorDto.EmailAddress;
        @operator.Address = operatorDto.Address;
        @operator.ContactName = operatorDto.ContactName;
        @operator.ProtocolType = (short)operatorDto.ProtocolType;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteOperatorAsync(Guid operatorId, CancellationToken cancellationToken)
    {
        var @operator = await context.Operators.FindAsync([operatorId], cancellationToken)
            ?? throw new NotFoundException(nameof(Operator), $"{operatorId}");

        context.Operators.Remove(@operator);
        await context.SaveChangesAsync(cancellationToken);
    }
}
