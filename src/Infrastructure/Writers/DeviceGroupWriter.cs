﻿namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class DeviceGroupWriter(IApplicationDbContext context) : IDeviceGroupWriter
{
    public async Task<DeviceGroupVm> CreateDeviceGroupAsync(DeviceGroupDto deviceGroupDto, CancellationToken cancellationToken)
    {
        var deviceGroup = new DeviceGroup
        {
            DeviceId = deviceGroupDto.DeviceId,
            GroupId = deviceGroupDto.GroupId
        };

        await context.DeviceGroups.AddAsync(deviceGroup, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new DeviceGroupVm(
            deviceGroup.DeviceId,
            deviceGroup.GroupId);
    }

    public async Task DeleteDeviceGroupAsync(Guid deviceId, long groupId, CancellationToken cancellationToken)
    {
        var deviceGroup = await context.DeviceGroups.FindAsync([deviceId, groupId], cancellationToken)
            ?? throw new NotFoundException(nameof(DeviceGroup), $"{deviceId},{groupId}");

        context.DeviceGroups.Remove(deviceGroup);
        await context.SaveChangesAsync(cancellationToken);
    }
}