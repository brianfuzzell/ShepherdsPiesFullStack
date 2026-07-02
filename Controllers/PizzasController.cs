using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShepherdsPiesControllers.DTOs;
using ShepherdsPiesControllers.Models;
using ShepherdsPiesControllers.Repositories;

namespace ShepherdsPiesControllers.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class PizzasController : ControllerBase
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public PizzasController(IPizzaRepository pizzaRepository, IOrderRepository orderRepository, IMapper mapper)
    {
        _pizzaRepository = pizzaRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    [HttpPost("orders/{orderId}/pizzas")]
    public async Task<ActionResult<PizzaResponseDto>> AddPizza(int orderId, PizzaCreateDto pizzaCreateDto)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order is null)
        {
            return NotFound();
        }

        var pizza = _mapper.Map<Pizza>(pizzaCreateDto);
        pizza.OrderId = orderId;
        pizza.PizzaToppings = pizzaCreateDto.ToppingIds
            .Select(toppingId => new PizzaTopping { ToppingId = toppingId })
            .ToList();

        await _pizzaRepository.AddAsync(pizza);

        var createdPizza = await _pizzaRepository.GetByIdAsync(pizza.Id);
        return Ok(_mapper.Map<PizzaResponseDto>(createdPizza));
    }

    [HttpPut("pizzas/{id}")]
    public async Task<ActionResult<PizzaResponseDto>> UpdatePizza(int id, PizzaUpdateDto pizzaUpdateDto)
    {
        var pizza = await _pizzaRepository.GetByIdAsync(id);
        if (pizza is null)
        {
            return NotFound();
        }

        pizza.SizeId = pizzaUpdateDto.SizeId;
        pizza.CheeseOptionId = pizzaUpdateDto.CheeseOptionId;
        pizza.SauceOptionId = pizzaUpdateDto.SauceOptionId;
        pizza.PizzaToppings = pizzaUpdateDto.ToppingIds
            .Select(toppingId => new PizzaTopping { ToppingId = toppingId })
            .ToList();

        await _pizzaRepository.UpdateAsync(pizza);

        var updatedPizza = await _pizzaRepository.GetByIdAsync(id);
        return Ok(_mapper.Map<PizzaResponseDto>(updatedPizza));
    }

    [HttpDelete("pizzas/{id}")]
    public async Task<IActionResult> DeletePizza(int id)
    {
        var pizza = await _pizzaRepository.GetByIdAsync(id);
        if (pizza is null)
        {
            return NotFound();
        }

        await _pizzaRepository.DeleteAsync(id);
        return NoContent();
    }
}