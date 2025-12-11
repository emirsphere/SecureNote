namespace SecureNote.Application.DTOs
{
    // DTO: Sadece veri taşıyan aptal kutulardır. İş mantığı barındırmazlar.
    // Kullanıcı kayıt olurken bize sadece bunları verebilir.
    public class RegisterRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}