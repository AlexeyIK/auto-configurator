namespace Data.Model
{
    public class Piece
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public int AutomobileId { get; set; }
        public string Image { get; set; } = null!;
        public string ModelUrl { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Manufacturer Manufacturer { get; set; } = null!;
        public Automobile Automobile { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
