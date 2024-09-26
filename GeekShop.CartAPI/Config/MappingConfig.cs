using AutoMapper;
using GeekShop.CartAPI.Data.ValueObjects;
using GeekShop.CartAPI.Model;

namespace GeekShop.CartAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                //Quando vai do cliente para o BD
                //config.CreateMap<ProductVO, Product>();

                //Quando vai do BD para o cliente
                //config.CreateMap<Product, ProductVO>();

                //Em vez de fazer de VO para objeto e dps de objeto para VO, posso fazer da seguinte forma com o .ReverseMap()
                config.CreateMap<ProductVO, Product>().ReverseMap();
                config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
                config.CreateMap<CartVO, Cart>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
