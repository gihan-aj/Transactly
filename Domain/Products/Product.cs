using System;

namespace Domain.Products
{
    public class Product
    {
        private Product(ProductId id, string name, Sku sku)
        {
            Id = id;
            Name = name;
            Sku = sku;
        }

        public ProductId Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public Sku Sku { get; private set; }

        public static Product Create(string name, Sku sku)
        {
            return new Product(new ProductId(Guid.NewGuid()) ,name, sku);
        }
    }
}
