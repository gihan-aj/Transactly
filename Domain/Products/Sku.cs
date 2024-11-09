namespace Domain.Products
{
    // Stock keeping unit
    public record Sku
    {
        // has a fixed length
        public const int DefaultLength = 8;

        private Sku(string value) => Value = value;

        public string Value { get; init; }

        public static Sku? Create(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if(value.Length != DefaultLength)
            {
                return null;
            }

            return new Sku(value);
        }
    }
}
