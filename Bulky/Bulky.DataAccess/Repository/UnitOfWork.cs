using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; set; }
        public IProductRepository Products { get; set; }
        public ICompanyRepository Companies { get; set; }
        public IApplicationUserRepository User { get; set; }
        public IShoppingCartRepository ShoppingCart { get; set; }
        public IOrderHeaderRepository OrderHeader { get; set; }
        public IOrderDetailsRepository OrderDetails { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Products = new ProductRepository(_db);
            Companies = new CompanyRepository(_db);
            User = new ApplicationUserRepository(_db); 
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
             
        }
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
