using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShepherdsPiesControllers.DTOs;
using ShepherdsPiesControllers.Repositories;

namespace ShepherdsPiesControllers.Controllers;

[ApiController]
[Route("api/sizes")]
[Authorize]
public class SizesController : ControllerBase
{
    private readonly ISizeRepository _sizeRepository;
    private readonly IMapper _mapper;

    public SizesController(ISizeRepository sizeRepository, IMapper mapper)
    {
        _sizeRepository = sizeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<SizeDto>>> GetSizes()
    {
        var sizes = await _sizeRepository.GetAllAsync();
        return Ok(_mapper.Map<List<SizeDto>>(sizes));
    }
}