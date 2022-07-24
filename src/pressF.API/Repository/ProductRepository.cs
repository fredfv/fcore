using pressF.API.Model;
using pressF.API.Repository.Interfaces;

namespace pressF.API.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoContext context) : base(context)
        {
        }
    }
}