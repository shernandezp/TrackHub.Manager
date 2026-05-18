namespace TrackHub.Manager.Domain.Models;

public readonly record struct DriverVm(Guid DriverId, Guid AccountId, string Name, string? Phone, string? DocumentType, string? DocumentNumber, bool Active, string? EmployeeCode, string? LicenseNumber, DateOnly? LicenseExpiresAt, Guid? DefaultTransporterId, DateTimeOffset LastModified);

public readonly record struct DriverAssignmentVm(Guid DriverId, Guid AccountId, string ResourceType, string ResourceId, bool Active);
