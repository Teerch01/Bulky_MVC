using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db), IProductRepository
{
    public void Update(Product product)
    {
        db.Products.Update(product);
    }
}
