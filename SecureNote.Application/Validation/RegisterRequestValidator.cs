using FluentValidation;
using SecureNote.Application.DTOs;
using System.Text.RegularExpressions;

namespace SecureNote.Application.Validators
{

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            // Kullanıcı Adı Kuralları
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş geçilemez.")
                .Must(username => !string.IsNullOrWhiteSpace(username)).WithMessage("Kullanıcı adı boş geçilemez.")
                .Length(3, 50).WithMessage("Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.")
                .Must(IsValidUsername).WithMessage("Kullanıcı adı geçersiz karakterler içeriyor.");

            // Email Kuralları
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi gereklidir.")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");

            // Şifre Kuralları
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .Must(password => !string.IsNullOrWhiteSpace(password)).WithMessage("Şifre boş geçilemez.")
                .Must(IsValidPassword).WithMessage("Şifre geçersiz.")
                .Must(SifreGucluMu).WithMessage("Şifreniz en az bir büyük harf ve bir rakam içermelidir.");
        }

        private bool SifreGucluMu(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            bool hasUpperCase = false;
            bool hasDigit = false;

            
            foreach (char c in password)
            {
                    if (char.IsUpper(c)) hasUpperCase = true;
                    if (char.IsDigit(c)) hasDigit = true;
                    if(hasUpperCase && hasDigit) break;
            }

            return hasUpperCase && hasDigit;
        }
        private bool IsValidUsername(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            if (input.Any(char.IsWhiteSpace)) return false;

            var hasScriptTag = Regex.IsMatch(input, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", RegexOptions.IgnoreCase);
            var hasJavaScriptLiteral = input.Contains("javascript:", StringComparison.OrdinalIgnoreCase);
            return !hasScriptTag && !hasJavaScriptLiteral;
        }
        private bool IsValidPassword(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            var hasScriptTag = Regex.IsMatch(input, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", RegexOptions.IgnoreCase);
            var hasJavaScriptLiteral = input.Contains("javascript:", StringComparison.OrdinalIgnoreCase);
            return !hasScriptTag && !hasJavaScriptLiteral;

        }
    }
}