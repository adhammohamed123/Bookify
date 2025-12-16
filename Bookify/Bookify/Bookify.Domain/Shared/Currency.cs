namespace Bookify.Domain.Shared
{
    public record Currency
    {
        internal static Currency None=>new Currency(string.Empty);
        public static Currency USD => new Currency("usd");
        public static Currency EUR => new Currency("eur");
        public static Currency[] All => [USD, EUR]; 
        public string Code { get; private set; }

        private Currency(string code)
        {
            Code = code;
        }

        public static Currency FromCode(string code)
        =>All.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase)) 
            ?? throw new InvalidOperationException("Not Supported Currency Code");
            
        
    }

}
