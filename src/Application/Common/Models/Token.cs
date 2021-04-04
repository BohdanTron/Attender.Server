using System;

namespace Attender.Server.Application.Common.Models
{
    public class Token
    {
        public Token(string value, DateTime expires) 
            => (Value, Expires) = (value, expires);

        public string Value { get; }
        public DateTime Expires { get; }
    }
}
