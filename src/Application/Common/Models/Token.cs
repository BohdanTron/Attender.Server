using System;
using System.ComponentModel.DataAnnotations;

namespace Attender.Server.Application.Common.Models
{
    public record Token
    {
        public Token(string value, DateTime expires)
            => (Value, Expires) = (value, expires);

        [Required]
        public string Value { get; }

        [Required]
        public DateTime Expires { get; set; }
    }
}
