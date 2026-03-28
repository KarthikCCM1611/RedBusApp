namespace WebAPI.Models
{
    public class RefreshToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = default!;
        public string TokenHash { get; set; } = default!;        // store SHA256 hash of token
        public string? ReplacedByTokenHash { get; set; }         // for rotation chain
        public DateTime ExpiresAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAtUtc { get; set; }              // if revoked
        public bool IsRevoked => RevokedAtUtc != null;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
    }
}
