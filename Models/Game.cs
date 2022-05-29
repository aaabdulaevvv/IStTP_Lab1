using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApplication1
{
    public partial class Game
    {
        [Display(Name = "Матч")]
        public int GId { get; set; }
        [Display(Name = "Домашня команда")]
        public string? GTeamH { get; set; }
        [Display(Name = "Гостьова команда")]
        public string? GTeamA { get; set; }
        [Display(Name = "Стадіон")]
        public string? GStadium { get; set; }
        [Display(Name = "Відвідуваність")]
        public int? GAttendance { get; set; }

        [Display(Name = "Стадіон")]
        public virtual Stadium? GStadiumNavigation { get; set; }
        [Display(Name = "Гостьова команда")]
        public virtual Team? GTeamANavigation { get; set; }
        [Display(Name = "Домашня команда")]
        public virtual Team? GTeamHNavigation { get; set; }
    }
}
