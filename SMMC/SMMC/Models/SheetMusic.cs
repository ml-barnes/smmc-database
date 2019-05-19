using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class SheetMusic
    {
        public SheetMusic()
        {
            SheetMusicTutor = new HashSet<SheetMusicTutor>();
        }

        public int SheetMusicId { get; set; }
        public string Title { get; set; }
        public string Composer { get; set; }
        public int Amount { get; set; }

        public ICollection<SheetMusicTutor> SheetMusicTutor { get; set; }
    }
}
