using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApplication1
{
    public partial class Team
    {
        public Team()
        {
            GameGTeamANavigations = new HashSet<Game>();
            GameGTeamHNavigations = new HashSet<Game>();
        }

        [Display(Name = "Команда")]
        public string TName { get; set; } = null!;
        [Display(Name = "Місто")]
        public string? TCity { get; set; }
        [Display(Name = "Стадіон")]
        public string? TStadium { get; set; }
        [Display(Name = "Тренер")]
        public string? TManager { get; set; }
        [Display(Name = "Код команди")]
        public string TId { get; set; } = null!;

        [Display(Name = "Місто")]
        public virtual City? TCityNavigation { get; set; }
        [Display(Name = "Стадіон")]
        public virtual Stadium? TStadiumNavigation { get; set; }
        public virtual ICollection<Game> GameGTeamANavigations { get; set; }
        public virtual ICollection<Game> GameGTeamHNavigations { get; set; }
    }
}
