namespace TrackHub.Manager.Domain.Models;
public record struct CategoryVm (
    Guid CategoryId,
    string Name,
    string? Description,
    CategoryType Type);
