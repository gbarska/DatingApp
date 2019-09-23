using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.UI.Controller
{
    public class MainController : Microsoft.AspNetCore.Mvc.Controller
    {
        // private readonly HeroesHandler _handler;

        // [HttpPost]
        // [Route("heroes/register")]
        // public ICommandResult Post([FromBody] RegisterHeroesCommand command)
        // {
        //     return _handler.Handle(command);
        // }
        
        [HttpGet]
        [Route("main")]
        public string Get()
        {
            return "hello world";
        }

        // [HttpGet]
        // [Route("heroes/{Id}")]
        // public GetHeroQueryResult Get(string Id)
        // {
        //     return _handler.Get(Id);
        // }

        // [HttpPut]
        // [Route("heroes/update")] 
        // public ICommandResult Put([FromBody] UpdateHeroesCommand command)
        // {
        //     return _handler.Handle(command);
        // }

        // [HttpDelete]
        // [Route("heroes/{Id}")]
        // public ICommandResult Delete(string Id)
        // {
        //     DeleteHeroCommand command = new DeleteHeroCommand(Id);
        //     return _handler.Handle(command);
        // }
    }
}