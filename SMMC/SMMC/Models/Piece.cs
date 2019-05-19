using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Piece
    {
        public Piece()
        {
            PerformancePiece = new HashSet<PerformancePiece>();
        }

        public int PieceId { get; set; }
        public string Title { get; set; }
        public string Composer { get; set; }
        public DateTime? LastPerformedDate { get; set; }

        public ICollection<PerformancePiece> PerformancePiece { get; set; }
    }
}
