namespace API.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }

        public bool IsActive => RevokedOn == null && !IsExpired;
        public bool IsExpired => ExpiresOn <= DateTime.UtcNow;

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
