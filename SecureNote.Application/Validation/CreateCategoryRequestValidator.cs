using FluentValidation;
using SecureNote.Application.DTOs;

namespace SecureNote.Application.Validation
{
    public class CreateCategoryRequestValidator: AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Not başlığı boş olamaz.")
                .Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage("Not başlığı boş olamaz.")
                .Length(1, 100).WithMessage("Not başlığı maksimum 100 karakter olabilir.");
        }
    }
}
