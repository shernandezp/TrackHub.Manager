using Common.Application.Attributes;
using Common.Domain.Constants;
using TrackHub.Manager.Application.Categories.Events;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Application.Categories.Commands.CreateCategory;

[Authorize(Resource = Resources.SettingsScreen, Action = Actions.View)]
public record CreateCategoryCommand : IRequest<CategoryVm>
{
    public required CategoryDto Category { get; set; }
}

public class CreateCategoryCommandHandler(ICategoryWriter writer, IPublisher publisher) : IRequestHandler<CreateCategoryCommand, CategoryVm>
{
    public async Task<CategoryVm> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await writer.CreateCategoryAsync(request.Category, cancellationToken);
        await publisher.Publish(new CategoryCreated.Notification(category.CategoryId), cancellationToken);
        return category;
    }
}
