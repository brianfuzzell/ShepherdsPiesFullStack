using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShepherdsPiesControllers.DTOs;
using ShepherdsPiesControllers.Repositories;

namespace ShepherdsPiesControllers.Controllers;

[ApiController]
[Route("api/toppings")]
[Authorize]
public class ToppingsController : ControllerBase
{
    private readonly IToppingRepository _toppingRepository;
    private readonly IMapper _mapper;

    public ToppingsController(IToppingRepository toppingRepository, IMapper mapper)
    {
        _toppingRepository = toppingRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ToppingDto>>> GetToppings()
    {
        var toppings = await _toppingRepository.GetAllAsync();
        return Ok(_mapper.Map<List<ToppingDto>>(toppings));
    }
}