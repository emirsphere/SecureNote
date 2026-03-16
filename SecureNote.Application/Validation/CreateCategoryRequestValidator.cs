using FluentValidation;
using SecureNote.Application.DTOs;

namespace SecureNote.Application.Validation
{
    public class CreateCategoryRequestValidator: AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Kategori boş olamaz.")
                .Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage("Kategori boş olamaz.")
                .Length(1, 100).WithMessage("Kategori maksimum 100 karakter olabilir.");
        }
    }
}
