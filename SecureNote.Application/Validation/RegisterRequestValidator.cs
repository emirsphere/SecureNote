using FluentValidation;
using SecureNote.Application.DTOs;
using System.Text.RegularExpressions;

namespace SecureNote.Application.Validators
{
    // AbstractValidator<T> sınıfından miras alıyoruz.
    // T: Hangi sınıfı denetleyeceğimiz (RegisterRequest)
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            // Kullanıcı Adı Kuralları
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş geçilemez.")
                .Must(username => !string.IsNullOrWhiteSpace(username)).WithMessage("Kullanıcı adı boş geçilemez.")
                .Length(3, 50).WithMessage("Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.")
                .Must(IsValidPasswordOrUsername).WithMessage("Kullanıcı adı geçersiz karakterler içeriyor.");

            // Email Kuralları
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi gereklidir.")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");

            // Şifre Kuralları (FluentValidation'ın gücü burada!)
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .Must(password => !string.IsNullOrWhiteSpace(password)).WithMessage("Şifre boş geçilemez.")
                .Must(IsValidPasswordOrUsername).WithMessage("Şifre geçersiz karakterler içeriyor.")
                .Must(SifreGucluMu).WithMessage("Şifreniz en az bir büyük harf ve bir rakam içermelidir.");
        }

        // Özel Doğrulama Metodu (Custom Validation Logic)
        // Data Annotations ile bunu yapmak çok zordur, burada çocuk oyuncağıdır.
        private bool SifreGucluMu(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            // Basit bir kontrol: Büyük harf ve rakam var mı?
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
        private bool IsValidPasswordOrUsername(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            
            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c)) return false; // Boşluk karakteri var mı?
            }

            var hasScriptTag = Regex.IsMatch(input, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", RegexOptions.IgnoreCase);
            var hasJavaScriptLiteral = input.Contains("javascript:", StringComparison.OrdinalIgnoreCase);
            return !hasScriptTag && !hasJavaScriptLiteral;
        }
    }
}