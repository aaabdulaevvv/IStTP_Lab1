using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartStController : ControllerBase
    {
        private readonly DBLibraryContext _context;
        public ChartStController(DBLibraryContext context)
        {
            _context = context;
        }

        [HttpGet("JsonDataSt")]
        public JsonResult JsonDataSt()
        {
            var stadia = _context.Stadia.ToList();
            List<object> staCapacity = new List<object>();
            staCapacity.Add(new[] { "Стадіон", "Вміщує" });
            foreach (var s in stadia)
            {
                staCapacity.Add(new object[] { s.StName, s.StCapacity });

            }
            return new JsonResult(staCapacity);
        }
    }
}
