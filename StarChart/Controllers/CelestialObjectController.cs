using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject object1)
        {
            _context.CelestialObjects.Add(object1);

            return CreatedAtAction("GetById", new { Id = object1.Id }, object1);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject object1)
        {
            var entity = _context.CelestialObjects.Find(id);
            if (entity == null)
                return NotFound();

            entity.Name = object1.Name;
            entity.OrbitalPeriod = object1.OrbitalPeriod;
            entity.OrbitedObjectId = object1.OrbitedObjectId;
            _context.CelestialObjects.Update(entity);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var entity = _context.CelestialObjects.Find(id);
            if (entity == null)
                return NotFound();

            entity.Name = name;
            _context.CelestialObjects.Update(entity);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _context.CelestialObjects.Where(x=>x.Id==id || x.OrbitedObjectId==id);
            if (!entity.Any())
                return NotFound();
            _context.CelestialObjects.RemoveRange(entity);
            _context.SaveChanges();
            return NoContent();
        }








    }
}
