namespace DotnetApiDemo.Dto
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public DateTime Created { get; set; }
    }
}
