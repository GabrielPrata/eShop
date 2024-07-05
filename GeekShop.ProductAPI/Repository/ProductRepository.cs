using AutoMapper;
using GeekShop.ProductAPI.Data.ValueObjects;
using GeekShop.ProductAPI.Model;
using GeekShop.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GeekShop.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySQLContext _context;
        private IMapper _mapper;

        public ProductRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductVO>> FindAll()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductVO>>(products);
        }

        public async Task<ProductVO> FindById(long id)
        {
            //Se não encontrar, retorna uma instância vazia
            Product product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Create(ProductVO vo)
        {
            //Faço a conversão do VO (DTO) para objeto e adiciono ao contexto
            Product product = _mapper.Map<Product>(vo);
            _context.Products.Add(product);

            //Salvo a operação no banco de dados
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Update(ProductVO vo)
        {
            //Faço a conversão do VO (DTO) para objeto e adiciono ao contexto
            Product product = _mapper.Map<Product>(vo);
            _context.Products.Update(product);

            //Salvo a operação no banco de dados
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                //Se não encontrar, retorna uma instância vazia
                Product product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync() ?? new Product();
                
                //Não tem como o objeto ser nulo, pois ele foi setado na linha acima, por isso faço a verificação pelo ID
                if(product.Id <= 0) return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
