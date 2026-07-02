using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShepherdsPiesControllers.DTOs;
using ShepherdsPiesControllers.Models;
using ShepherdsPiesControllers.Repositories;

namespace ShepherdsPiesControllers.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrdersController(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder(OrderCreateDto orderCreateDto)
    {
        if (orderCreateDto.OrderType == "DineIn" && orderCreateDto.TableNumber is null)
        {
            return BadRequest("TableNumber is required for dine-in orders.");
        }

        var order = _mapper.Map<Order>(orderCreateDto);
        order.OrderDate = DateTime.UtcNow;
        order.IsCancelled = false;

        await _orderRepository.AddAsync(order);

        var createdOrder = await _orderRepository.GetByIdAsync(order.Id);
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, _mapper.Map<OrderResponseDto>(createdOrder));
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDto>>> GetOrders([FromQuery] DateTime? date)
    {
        var targetDate = date.HasValue
            ? DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Utc)
            : DateTime.UtcNow.Date;
        var orders = await _orderRepository.GetByDateAsync(targetDate);
        return Ok(_mapper.Map<List<OrderResponseDto>>(orders));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<OrderResponseDto>(order));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderResponseDto>> UpdateOrder(int id, OrderUpdateDto orderUpdateDto)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        order.DeliveryEmployeeId = orderUpdateDto.DeliveryEmployeeId;
        await _orderRepository.UpdateAsync(order);

        var updatedOrder = await _orderRepository.GetByIdAsync(id);
        return Ok(_mapper.Map<OrderResponseDto>(updatedOrder));
    }
}