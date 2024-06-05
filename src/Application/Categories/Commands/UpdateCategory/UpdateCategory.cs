using TrackHub.Manager.Application.Categories.Events;

namespace TrackHub.Manager.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand : IRequest
{
    public CategoryDto Category { get; set; }
}

public class UpdateCategoryCommandHandler(ICategoryWriter writer, IPublisher publisher) : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        await writer.UpdateCategoryAsync(request.Category, cancellationToken);
        await publisher.Publish(new CategoryUpdated.Notification(request.Category.CategoryId), cancellationToken);
    }
}
