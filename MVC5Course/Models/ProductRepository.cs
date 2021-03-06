using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
	{
        public Product Find(int id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);            
        }

        public override IQueryable<Product> All()
        {
            return base.All().Where(p=>p.IsDelete ==false);
        }

        public IQueryable<Product> All(bool isAll)
        {
            if (isAll)
            {
                return this.All();
            }
            else
            {
                return base.All();
            }
        }

        public override void Delete(Product entity)
        {
            entity.IsDelete = true;
        }
    }

	public  interface IProductRepository : IRepository<Product>
	{

	}
}