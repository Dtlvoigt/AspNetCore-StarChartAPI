using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        /*public IActionResult Index()
        {
            return View();
        }*/

        public CelestialObjectController(ApplicationDbContext context)
        {
            context = _context;
        }
    }
}
