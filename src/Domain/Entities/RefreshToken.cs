using System;

namespace Attender.Server.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; } = default!;
        public string AccessTokenId { get; set; } = default!;
        public bool Used { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}