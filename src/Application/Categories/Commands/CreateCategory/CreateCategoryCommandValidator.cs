using TrackHub.Manager.Application.Categories.Commands.CreateCategory;

namespace TrackHub.Manager.Application.TodoItems.Commands.CreateTodoItem;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(v => v.Category.Name)
            .MaximumLength(ColumnMetadata.DefaultNameLength)
            .NotEmpty();

        RuleFor(v => v.Category.Description)
            .MaximumLength(ColumnMetadata.DefaultDescriptionLength)
            .NotEmpty();
    }
}
