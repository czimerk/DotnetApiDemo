namespace DotnetApiDemo.Dto
{
    public class FilterDto
    {
        public string Name { get; set; }
        public double? PriceGreaterThan { get; set; }
        public double? PriceLessThan { get; set; }
    }
}
