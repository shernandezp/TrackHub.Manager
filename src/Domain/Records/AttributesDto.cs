namespace TrackHub.Manager.Domain.Records;

public readonly record struct AttributesDto(
    bool? Ignition,
    int? Satellites,
    double? Mileage,
    double? HobbsMeter,
    double? Temperature
    );
