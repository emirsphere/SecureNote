using FluentValidation;
using SecureNote.Application.DTOs;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SecureNote.Application.Validation
{
    public class CreateNoteValidator: AbstractValidator<NoteDto>
    {
        public CreateNoteValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Not başlığı boş olamaz.")
                .Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage("Not başlığı boş olamaz.")
                .Length(1, 100).WithMessage("Not başlığı maksimum 100 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Not içeriği boş olamaz.")
                .Must(content => !string.IsNullOrWhiteSpace(content)).WithMessage("Not içeriği boş olamaz.")
                .Must(IsValidContent).WithMessage("Hatalı kullanım.")
                .Length(1, 2000).WithMessage("Not içeriği maksimum 2000 karakter olabilir.");
                
        }

        private bool IsValidContent(string content)
        {
            if(string.IsNullOrEmpty(content)) return false;

            var hasScriptTag = Regex.IsMatch(content, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", RegexOptions.IgnoreCase);
            var hasJavaScriptLiteral = content.Contains("javascript:", StringComparison.OrdinalIgnoreCase);

            return !hasScriptTag && !hasJavaScriptLiteral;
        }



    }
}