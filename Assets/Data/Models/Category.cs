namespace Data.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public string Image { get; set; } = null!;
    }
}
