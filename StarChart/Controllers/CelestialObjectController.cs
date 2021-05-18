using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObj = _context.CelestialObjects.Find(id);

            if (celestialObj == null)
                return NotFound();

            celestialObj.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
            return Ok(celestialObj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjs = _context.CelestialObjects.Where(e => e.Name == name);

            if (!celestialObjs.Any())
                return NotFound();

            foreach(var celestialObj in celestialObjs)
            {
                celestialObj.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObj.Id).ToList();
            }
            return Ok(celestialObjs.ToList());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObj in celestialObjects)
            {
                celestialObj.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObj.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObjectInput)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
                return NotFound();

            celestialObject.Name = celestialObjectInput.Name;
            celestialObject.OrbitalPeriod = celestialObjectInput.OrbitalPeriod;
            celestialObject.OrbitedObjectId = celestialObjectInput.OrbitedObjectId;

            _context.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
                return NotFound();

            celestialObject.Name = name;
            _context.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id || e.Id == id).ToList();
            if (!celestialObjects.Any())
                return NotFound();

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
