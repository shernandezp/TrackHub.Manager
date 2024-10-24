namespace TrackHub.Manager.Domain.Models;

public readonly record struct AttributesVm(
    bool? Ignition,
    int? Satellites,
    double? Mileage,
    double? HobbsMeter,
    double? Temperature
    );

