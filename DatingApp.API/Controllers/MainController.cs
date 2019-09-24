using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.UI.Controller
{
     [Route("api/[controller]")]
     [ApiController]

     //the ControllerBase doesn't offer view support it's perfect for API only 
    public class MainController : ControllerBase
    {
         
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult Create()
        {
            return Ok();  
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok();
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