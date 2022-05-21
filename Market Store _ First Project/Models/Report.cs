using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Store___First_Project.Models
{
    public class Report
    {
        private readonly ModelContext _context = new ModelContext();

        public int GetRegisteredUsers()
        {
            if (_context.UserLogin.Where(user => user.RoleId == 1).Count() == 0)
                return 0;

            return _context.UserLogin.Where(user => user.RoleId == 1).Count();
        }

        public double GetTotalSales(int storeId)
        {

          var userOrder =  _context.Userorder.Where(uo => uo.IsCheckout == true).ToList();
          var productStore = _context.ProductStore.Where(ps => ps.Storeid == storeId).ToList();
            var productOrder = _context.Productorder.ToList();

            var q = from uOrder in userOrder
                    join pOrder in productOrder
                    on uOrder.Id equals pOrder.Orderid
                    join pStore in productStore
                    on pOrder.Productid equals pStore.Id
                    select new MultiTables
                    {
                        userorder = uOrder, productorder = pOrder , productStore = pStore
                    };
            double total = 0;
                  
            foreach(var item in q)
            {
                total += (double)item.userorder.Cost;
            }

            return total;

        }
    }
}
