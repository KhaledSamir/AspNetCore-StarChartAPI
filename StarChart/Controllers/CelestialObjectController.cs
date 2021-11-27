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

            if (celestial == null)
                return NotFound();

            return Ok(celestial);
        }

        [HttpGet("{name:string}", Name = "GetByName")]

        public IActionResult GetByName(string name)
        {
            var celestials = _context.CelestialObjects.Where(c => c.Name == name).ToList();

            if (celestials == null)
                return NotFound();

            celestials.ForEach(celes =>
            {
                celes.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celes.Id).ToList();
            });

            return Ok(celestials);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.CelestialObjects);
        }
    }
}
