namespace GraphQLDemo.Domain
{
    public class OrderLine
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Article Article { get; set; }
        public Guid OrderId { get; set; }
    }
}
