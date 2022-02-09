using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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
            var entity =_context.CelestialObjects.Find(id);
            if (entity == null)
                return NotFound();
            entity.Satellites = _context.CelestialObjects.Where(x => x.Id == id).ToList();
            return Ok(entity);
        }

        [HttpGet("{name}", Name = "GetByName ")]
        public IActionResult GetByName(string name)
        {
            var entity = _context.CelestialObjects.Where(e => e.Name == name);
            if (!entity.Any())
                return NotFound();
            foreach (var celestialObject in entity)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(entity.ToList());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entity = _context.CelestialObjects.ToList();
            foreach (var celestialObject in entity)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(entity);
        }

       
    }
}
