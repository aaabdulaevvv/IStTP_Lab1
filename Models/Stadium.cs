using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApplication1
{
    public partial class Stadium
    {
        public Stadium()
        {
            Games = new HashSet<Game>();
            Teams = new HashSet<Team>();
        }

        [Display(Name = "Стадіон")]
        public string StName { get; set; } = null!;
        [Display(Name = "Місто")]
        public string? StCity { get; set; }
        [Display(Name = "Вміщує")]
        public int? StCapacity { get; set; }
        [Display(Name = "Код стадіону")]
        public string StId { get; set; } = null!;

        [Display(Name = "Місто")]
        public virtual City? StCityNavigation { get; set; }
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
