using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository;

public class UnitOfWork(ApplicationDbContext db) : IUnitOfWork
{
    public ICategoryRepository Category { get; private set; } = new CategoryRepository(db);

    public IProductRepository Product { get; private set; } = new ProductRepository(db);

    public void Save()
    {
        db.SaveChanges();
    }
}
