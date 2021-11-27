using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestial = _context.CelestialObjects.Find(id);

            if (celestial == null)
                return NotFound();

            celestial.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id).ToList();

            return Ok(celestial);
        }

        [HttpGet("{name}")]

        public IActionResult GetByName(string name)
        {
            var celestials = _context.CelestialObjects.Where(c => c.Name == name).ToList();

            if (!celestials.Any())
                return NotFound();

            foreach (var celestialObject in celestials)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestials);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestials = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestials)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestials);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            var newCeles = new CelestialObject { Id = celestialObject.Id };

            return CreatedAtRoute("GetById", newCeles.Id, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var existCeles = _context.CelestialObjects.Find(id);

            if (existCeles == null)
                return NotFound();

            existCeles.Name = celestialObject.Name;
            existCeles.OrbitalPeriod = celestialObject.OrbitalPeriod;
            existCeles.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(existCeles);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existCeles = _context.CelestialObjects.Find(id);
            if (existCeles == null)
                return NotFound();

            existCeles.Name = name;
            _context.CelestialObjects.Update(existCeles);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existCeles = _context.CelestialObjects.Find(id);
            if (existCeles == null)
                return NotFound();
            _context.CelestialObjects.RemoveRange(existCeles);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
