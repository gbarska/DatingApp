using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.UI.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    //the ControllerBase doesn't offer view support it's perfect for API only 
    public class MainController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MainController(AppDbContext context)
        {
            this._context = context;

        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var values = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var values = await _context.Values.ToArrayAsync();
            return Ok(values);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var values = await _context.Values.ToListAsync();
            return Ok(values);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id)
        {
            return Ok();
        }

    }
}