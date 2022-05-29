using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApplication1
{
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        [Display(Name = "Країна")]
        public string CoName { get; set; } = null!;
        [Display(Name = "Населення (млн. осіб)")]
        public int? CoPopulation { get; set; }
        [Display(Name = "Код країни")]
        public string CoId { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; }
    }
}
