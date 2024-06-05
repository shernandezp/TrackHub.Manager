using TrackHub.Manager.Application.Categories.Events;

namespace TrackHub.Manager.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest;

public class DeleteCategoryCommandHandler(ICategoryWriter writer, IPublisher publisher) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await writer.DeleteCategoryAsync(request.Id, cancellationToken);
        await publisher.Publish(new CategoryDeleted.Notification(request.Id), cancellationToken);
    }
}
