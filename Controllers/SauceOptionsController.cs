using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShepherdsPiesControllers.DTOs;
using ShepherdsPiesControllers.Repositories;

namespace ShepherdsPiesControllers.Controllers;

[ApiController]
[Route("api/sauceoptions")]
[Authorize]
public class SauceOptionsController : ControllerBase
{
    private readonly ISauceOptionRepository _sauceOptionRepository;
    private readonly IMapper _mapper;

    public SauceOptionsController(ISauceOptionRepository sauceOptionRepository, IMapper mapper)
    {
        _sauceOptionRepository = sauceOptionRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<SauceOptionDto>>> GetSauceOptions()
    {
        var sauceOptions = await _sauceOptionRepository.GetAllAsync();
        return Ok(_mapper.Map<List<SauceOptionDto>>(sauceOptions));
    }
}