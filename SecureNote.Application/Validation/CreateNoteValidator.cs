using FluentValidation;
using SecureNote.Application.DTOs;


namespace SecureNote.Application.Validation
{
    public class CreateNoteValidator: AbstractValidator<NoteDto>
    {
        public CreateNoteValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Not başlığı boş olamaz.")
                .Length(1, 100).WithMessage("Not başlığı maksimum 100 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Not içeriği boş olamaz.")
                .Length(1, 2000).WithMessage("Not içeriği maksimum 2000 karakter olabilir.");


        }
    }
}