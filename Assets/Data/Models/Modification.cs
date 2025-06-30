namespace Data.Model
{
    public class Modification
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PieceId { get; set; }
        public int? ColorId { get; set; }

        public Project Project { get; set; }
        public Piece Piece { get; set; }
        public Color Color { get; set; }

        public Modification(int id, int projectId, int pieceId, int? colorId = null)
        {
            ProjectId = projectId;
            PieceId = pieceId;
            ColorId = colorId;
        }
    }
}
