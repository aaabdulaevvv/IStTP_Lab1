using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApplication1
{
    public partial class City
    {
        public City()
        {
            Stadia = new HashSet<Stadium>();
            Teams = new HashSet<Team>();
        }

        [Display(Name = "Місто")]
        public string CiName { get; set; } = null!;
        [Display(Name = "Країна")]
        public string? CiCountry { get; set; }
        [Display(Name = "Насленнея (тисяч осіб)")]
        public int? CiPopulation { get; set; }
        [Display(Name = "Скорочена назва")]
        public string CiId { get; set; } = null!;
        [Display(Name = "Країна")]

        public virtual Country? CiCountryNavigation { get; set; }
        public virtual ICollection<Stadium> Stadia { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
