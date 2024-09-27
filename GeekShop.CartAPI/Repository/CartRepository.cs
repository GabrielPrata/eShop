using AutoMapper;
using GeekShop.CartAPI.Data.ValueObjects;
using GeekShop.CartAPI.Model;
using GeekShop.CartAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShop.CartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly MySQLContext _context;
        private IMapper _mapper;

        public CartRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            throw new NotImplementedException();

        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cartHeader != null)
            {
                //Todos os cart details cujo o cart header id for == ao que foi recuperado acima serão removidos
                _context.CartDetails.RemoveRange(_context.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id));

                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CartVO> FindCartByUserId(string userId)
        {
            Cart cart = new Cart()
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId),
            };

            cart.CartDetails = _context.CartDetails.Where(c => c.CartHeaderId == cart.CartHeader.Id).Include(c => c.Product);

            return _mapper.Map<CartVO>(cart);
        }

        public async Task<bool> RemoveFromCart(long cartDetailsId)
        {
            try
            {
                CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(c => c.Id == cartDetailsId);

                int total = _context.CartDetails.Where(c => c.CartHeaderId == cartDetail.CartHeaderId).Count();

                _context.CartDetails.Remove(cartDetail);


                //Limpa o carrinho do banco
                if(total == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CartVO> SaveOrUpdateCart(CartVO vo)
        {
            Cart cart = _mapper.Map<Cart>(vo);

            //Busco o produto a ser adicionado
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == vo.CartDetails.FirstOrDefault().ProductId);

            //Se o produto for nulo faço o cadastro
            if (product == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }

            //AsNoTracking serve para dizer para o Entity que as mudanças não devem ser realizadas na base
            var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

            if (cartHeader == null)
            {
                //Salvo o header
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();

                //Passar para um método dps
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;

                //Seto como nulo pois o produto ja esta no contexto
                cart.CartDetails.FirstOrDefault().Product = null;

                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());

                await _context.SaveChangesAsync();
            }
            else
            {
                //Se o header não esta nulo, atualizo as informações
                //Verifica se CartDetail tem o mesmo produto
                var cartDetail = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(p =>
                    p.ProductId == cart.CartDetails.FirstOrDefault().ProductId
                    && p.CartHeaderId == cartHeader.Id
                );

                if (cartDetail == null)
                {
                    //Crio o cart detail
                    //Passar para um método dps
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                }
                else
                {
                    //Se não for nulo, atualizo o cart detail
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                    cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;

                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }

            }

            return _mapper.Map<CartVO>(cart);
        }
    }
}
