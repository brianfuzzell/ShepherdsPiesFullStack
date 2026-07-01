using AutoMapper;
using ShepherdsPiesControllers.DTOs;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Size, SizeDto>();
        CreateMap<CheeseOption, CheeseOptionDto>();
        CreateMap<SauceOption, SauceOptionDto>();
        CreateMap<Topping, ToppingDto>();

        CreateMap<Pizza, PizzaResponseDto>()
            .ForMember(dest => dest.Toppings, opt => opt.MapFrom(src => src.PizzaToppings.Select(pt => pt.Topping)))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Size.Price + src.PizzaToppings.Count * 0.50m));

        // PizzaToppings requires the pizza's Id, which doesn't exist until after it's saved,
        // so join rows are built in the controller/repository, not here.
        CreateMap<PizzaCreateDto, Pizza>()
            .ForMember(dest => dest.PizzaToppings, opt => opt.Ignore());
        CreateMap<PizzaUpdateDto, Pizza>()
            .ForMember(dest => dest.PizzaToppings, opt => opt.Ignore());

        CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
            .ForMember(dest => dest.DeliveryEmployeeName, opt => opt.MapFrom(src =>
                src.DeliveryEmployee == null ? null : src.DeliveryEmployee.FirstName + " " + src.DeliveryEmployee.LastName))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src =>
                src.Pizzas.Sum(p => p.Size.Price + p.PizzaToppings.Count * 0.50m) + (src.OrderType == "Delivery" ? 5.00m : 0m)));

        CreateMap<OrderCreateDto, Order>();
        CreateMap<OrderUpdateDto, Order>();
    }
}