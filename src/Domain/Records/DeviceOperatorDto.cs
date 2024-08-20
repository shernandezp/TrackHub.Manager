﻿namespace TrackHub.Manager.Domain.Records;
public record struct DeviceOperatorDto(
    int Identifier,
    string Serial,
    Guid DeviceId, 
    Guid OperatorId
    );
