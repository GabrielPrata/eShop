using Microsoft.EntityFrameworkCore;

namespace GeekShop.ProductAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(){}

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }
    }
}
