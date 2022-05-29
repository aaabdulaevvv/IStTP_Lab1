using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication1.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DBLibraryContext _context;
        public ChartController(DBLibraryContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var countries = _context.Countries.ToList();
            List<object> countriesCity = new List<object>();
            countriesCity.Add(new[] { "Країна", "Населення" });
            foreach (var c in countries)
            {
                countriesCity.Add(new object[] { c.CoName, c.CoPopulation });
                
            }
            return new JsonResult(countriesCity);
        }
    }
}