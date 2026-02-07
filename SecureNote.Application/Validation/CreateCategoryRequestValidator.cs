using FluentValidation;
using SecureNote.Application.DTOs;

namespace SecureNote.Application.Validation
{
    public class CreateCategoryRequestValidator: AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Kategori adı boş geçilemez.")
                .Length(1,50).WithMessage("Kategori adı maksimum 50 karakter arasında olmalıdır.");
        }
    }
}
