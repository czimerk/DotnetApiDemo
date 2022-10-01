namespace GraphQLDemo.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public Status Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
