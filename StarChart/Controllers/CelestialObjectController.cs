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

        [HttpGet("{id:int}",Name = "GetById")]
        
        public IActionResult GetById(int id)
        {
            var celestial = _context.CelestialObjects.Find(id);

            celestial.Satellites = _context.CelestialObjects.Where(c => c.Name == celestial.Name).ToList();

            if (celestial == null)
                return NotFound();

            return Ok(celestial);
        }

        [HttpGet("{name}")]

        public IActionResult GetByName(string name)
        {
            var celestials = _context.CelestialObjects.Where(c => c.Name == name).ToList();

            if (celestials == null)
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
    }
}
