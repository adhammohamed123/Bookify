using System.Text.Json.Serialization;

namespace Bookify.Domain.Shared
{
    public record Currency
    {
        internal static Currency None=>new(string.Empty);
        public static Currency USD => new("usd");
        public static Currency EUR => new("eur");
        public static Currency[] All => [USD, EUR]; 
        public string Code { get; private set; }
        
        [JsonConstructor]
        private Currency(string code)
        {
            Code = code;
        }

        public static Currency FromCode(string code)
        =>All.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase)) 
            ?? throw new InvalidOperationException("Not Supported Currency Code");
            
        
    }

}
