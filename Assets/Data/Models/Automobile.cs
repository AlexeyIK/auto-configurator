namespace Data.Model
{
    public class Automobile
    {
        public int Id { get; set; }
        public int AutoBrandId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ModelUrl { get; set; }

        public AutoBrand AutoBrand { get; set; } = null!;
    }
}
