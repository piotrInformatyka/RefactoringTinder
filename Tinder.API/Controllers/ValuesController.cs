using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tinder.API.Data;

namespace Tinder.API.Controllers
{
    [Route("values")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
        public ValuesController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var values = _context.Values.ToList();
            return await Task.FromResult(Ok(values));
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var values = _context.Values.AsQueryable().FirstOrDefault(x => x.Id == id);
            return Ok(values);
        }
        [HttpPost]
        public IActionResult AddValue([FromBody] Value value)
        {
            _context.Values.Add(value);
            return NoContent();
        }

    }
}