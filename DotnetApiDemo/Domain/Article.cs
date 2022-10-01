namespace GraphQLDemo.Domain
{
    public class Article
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }

        public string Brand { get; set; }
        public int InventoryCode { get; set; }
        public DateTime Created { get; set; }
        public Guid StoreId { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
        public string ShortText { get; set; }
        public string LongText { get; set; }
        public string Type { get; set; }
    }
}
