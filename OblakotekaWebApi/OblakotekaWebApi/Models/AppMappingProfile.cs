using AutoMapper;
using OblakotekaWebApi.Models;

namespace OblakotekaWebApi;
public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Product, ProductViewModel>(); // Преобразование из Product в ProductViewModel
        CreateMap<ProductViewModel, Product>(); // Преобразование из ProductViewModel в Product
    }
}