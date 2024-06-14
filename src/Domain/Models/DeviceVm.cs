﻿namespace TrackHub.Manager.Domain.Models;
public readonly record struct DeviceVm(
    Guid DeviceId,
    string Identifier,
    string Name,
    DeviceType DeviceTypeId,
    string? Description
    );
