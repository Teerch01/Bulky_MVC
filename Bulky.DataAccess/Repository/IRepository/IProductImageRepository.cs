﻿using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        void Update(ProductImage productimage);
    }
}
