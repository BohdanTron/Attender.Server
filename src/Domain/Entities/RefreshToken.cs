using System;

namespace Attender.Server.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; } = string.Empty;
        public string AccessTokenId { get; set; } = string.Empty;
        public bool Used { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}