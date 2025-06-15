using FloreriaAPI_ASP.NET.DTOs;
using FloreriaAPI_ASP.NET.Filters;
using FloreriaAPI_ASP.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FloreriaAPI_ASP.NET.Controllers;

[ApiController]
[Route("[controller]")]
public class FloreriaController : ControllerBase
{
    private IFlowerService _flowerService;
    public FloreriaController(IFlowerService flowerService)
    {
        _flowerService = flowerService;
    }
    // GET all action
    [HttpGet]
    public async Task<List<Flower>> GetAll([FromQuery] FlowerFilter filter){
        return await _flowerService.GetAllFlowers(filter);
    }
    // GET by Id action
    [HttpGet("{id}")]
    public async Task<ActionResult<Flower>> Get(int id){
        Flower? f = await _flowerService.GetFlower(id);
        return (f is null) ? NotFound() : f;
    }
    // POST action
    [HttpPost]
    [Authorize(Policy = PermisosEnum.FlowerCreate)]
    public ActionResult Create([FromBody] FlowerDTO flor){
        _flowerService.CreateFlower(flor);
        return Created();
    }
    // PUT action
    [HttpPut("{id}")]
    public async Task<ActionResult<Flower>> Update(int id, [FromBody] FlowerDTO flor){
        if ((await _flowerService.GetFlower(id)) is null) return BadRequest();

        return await _flowerService.UpdateFlower(id, flor);
    }
    // DELETE action
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id){
        if ((await _flowerService.GetFlower(id)) is null) return BadRequest();

        return await _flowerService.DeleteFlower(id);
    }
}