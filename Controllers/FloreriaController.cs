using FloreriaAPI_ASP.NET.Models;
using FloreriaAPI_ASP.NET.Services;

using Microsoft.AspNetCore.Mvc;
namespace FloreriaAPI_ASP.NET.Controllers;

[ApiController]
[Route("[controller]")]
public class FloreriaController : ControllerBase{

    public FloreriaController(){}
    // GET all action
    [HttpGet]
    public List<Flor> GetAll(){
        return FlorService.GetAll();
    }
    // GET by Id action
    [HttpGet("{id}")]
    public ActionResult<Flor> Get(int id){
        Flor? f = FlorService.Get(id);
        return (f is null) ? NotFound() : f;
    }
    // POST action
    [HttpPost]
    public IActionResult Create(Flor flor){
        FlorService.Add(flor);
        return Created();
    }
    // PUT action
    [HttpPut("{id}")]
    public IActionResult Update(int id, Flor flor){
        if (flor.Id != id) return NotFound();
        if (FlorService.Get(id) is null) return BadRequest(); 

        flor.Id = id;
        FlorService.Update(flor);

        return NoContent();
    }
    // DELETE action
    [HttpDelete("{id}")]
    public IActionResult Delete(int id){
        if (FlorService.Get(id) is null) return BadRequest();

        FlorService.Delete(id);

        return NoContent();
    }
}