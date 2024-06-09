using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Products { get; }
        ICompanyRepository Companies { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderHeaderRepository  OrderHeader { get; }
        IApplicationUserRepository User { get; set; }
        void Save();
    }
}
