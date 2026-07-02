using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShepherdsPiesControllers.DTOs;
using ShepherdsPiesControllers.Repositories;

namespace ShepherdsPiesControllers.Controllers;

[ApiController]
[Route("api/cheeseoptions")]
[Authorize]
public class CheeseOptionsController : ControllerBase
{
    private readonly ICheeseOptionRepository _cheeseOptionRepository;
    private readonly IMapper _mapper;

    public CheeseOptionsController(ICheeseOptionRepository cheeseOptionRepository, IMapper mapper)
    {
        _cheeseOptionRepository = cheeseOptionRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CheeseOptionDto>>> GetCheeseOptions()
    {
        var cheeseOptions = await _cheeseOptionRepository.GetAllAsync();
        return Ok(_mapper.Map<List<CheeseOptionDto>>(cheeseOptions));
    }
}