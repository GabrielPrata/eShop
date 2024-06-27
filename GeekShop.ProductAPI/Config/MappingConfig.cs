using AutoMapper;
using GeekShop.ProductAPI.Data.ValueObjects;
using GeekShop.ProductAPI.Model;

namespace GeekShop.ProductAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                //Quando vai do cliente para o BD
                config.CreateMap<ProductVO, Product>();

                //Quando vai do BD para o cliente
                config.CreateMap<Product, ProductVO>();
            });
            return mappingConfig;
        }
    }
}
