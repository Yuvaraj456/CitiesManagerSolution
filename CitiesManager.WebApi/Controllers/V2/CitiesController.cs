using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebApi.Entities;
using CitiesManager.WebApi.Model;

namespace CitiesManager.WebApi.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Cities
        /// <summary>
        /// To get list of CityName from cities table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("Application/xml")]
        public async Task<ActionResult<IEnumerable<string?>>> GetCities()
        {
            if (_context.Cities == null)
            {
                return NotFound();
            }
            return await _context.Cities                
                .OrderBy(x=>x.CityName)
                .Select(x=>x.CityName)
                .ToListAsync();
        }

      
    }
}
