using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Store___First_Project.Models
{
    public class AdminReport
    {
        private readonly ModelContext _context = new ModelContext();

        public int GetRegisteredUsers()
        {
            if (_context.UserLogin.Where(user => user.RoleId == 1).Count() == 0)
                return 0;

            return _context.UserLogin.Where(user => user.RoleId == 1).Count();
        }
        public double GetTodaySale()
        {
            var userOrder = _context.Userorder.Where(uo => uo.IsCheckout == true
            && uo.Dateoforder.Value.TimeOfDay == DateTime.Now.TimeOfDay);

            if (userOrder.Count() == 0)
                return 0;
            return (double)userOrder.Sum(ur => ur.Cost);
        }
        public double GetMontlySale()
        {
            var userOrder = _context.Userorder.Where(uo => uo.IsCheckout == true
                       && uo.Dateoforder.Value.Month == DateTime.Now.Month);

            if (userOrder.Count() == 0)
                return 0;
            return (double)userOrder.Sum(ur => ur.Cost);
        }

        public double GetTotalSale()
        {
            var userOrder = _context.Userorder.Where(uo => uo.IsCheckout == true);

            if (userOrder.Count() == 0)
                return 0;
            return (double)userOrder.Sum(ur => ur.Cost);
        }

        public int GetTotalStore()
        {
            return _context.Store.Count();
        }

        public double GetToadySaleForStore(int storeId)
        {
            var userOrder = _context.Userorder.Where(uo => uo.IsCheckout == true
            && uo.Dateoforder.Value.TimeOfDay == DateTime.Now.TimeOfDay).ToList();
            var productStore = _context.ProductStore.Where(ps => ps.Storeid == storeId).ToList();
            var productOrder = _context.Productorder.ToList();

            var q = from uOrder in userOrder
                    join pOrder in productOrder
                    on uOrder.Id equals pOrder.Orderid
                    join pStore in productStore
                    on pOrder.Productid equals pStore.Id
                    select new MultiTables
                    {
                        userorder = uOrder,
                        productorder = pOrder,
                        productStore = pStore
                    };
            double total = 0;

            foreach (var item in q)
            {
                total += (double)item.userorder.Cost;
            }

            return total;
        }
        public double GetMonthlySaleForStore(int storeId)
        {
            var userOrder = _context.Userorder.Where(uo => uo.IsCheckout == true
            && uo.Dateoforder.Value.Month == DateTime.Now.Month).ToList();
            var productStore = _context.ProductStore.Where(ps => ps.Storeid == storeId).ToList();
            var productOrder = _context.Productorder.ToList();

            var q = from uOrder in userOrder
                    join pOrder in productOrder
                    on uOrder.Id equals pOrder.Orderid
                    join pStore in productStore
                    on pOrder.Productid equals pStore.Id
                    select new MultiTables
                    {
                        userorder = uOrder,
                        productorder = pOrder,
                        productStore = pStore
                    };
            double total = 0;

            foreach (var item in q)
            {
                total += (double)item.userorder.Cost;
            }

            return total;
        }
        public double GetTotalSalesForStore(int storeId)
        {

            var userOrder = _context.Userorder.Where(uo => uo.IsCheckout == true).ToList();
            var productStore = _context.ProductStore.Where(ps => ps.Storeid == storeId).ToList();
            var productOrder = _context.Productorder.ToList();

            var q = from uOrder in userOrder
                    join pOrder in productOrder
                    on uOrder.Id equals pOrder.Orderid
                    join pStore in productStore
                    on pOrder.Productid equals pStore.Id
                    select new MultiTables
                    {
                        userorder = uOrder,
                        productorder = pOrder,
                        productStore = pStore
                    };
            double total = 0;

            foreach (var item in q)
            {
                total += (double)item.userorder.Cost;


            }

            return total;

        }

        public int GetTotalProductForStore(int storeId)
        {
            var productStore = _context.ProductStore.Where(pr => pr.Storeid == storeId);
            if (productStore.Count() == 0)
                return 0;

            return productStore.Count();


        }

        public bool IsLoss(int orderId)
        {
            var userOrder = _context.Userorder.Where(uo => uo.IsCheckout == true
                && uo.Id == orderId).SingleOrDefault();

            int cost = (int)userOrder.Cost;
            int totalCost = 0;

            var productOrders = _context.Productorder.Where(pr => pr.Orderid == userOrder.Id).ToList();
            foreach (var productOrder in productOrders)
            {
                int qunitiy = (int)productOrder.Quntity;

                var productStore = _context.ProductStore.Where(ps => ps.Id == productOrder.Productid).SingleOrDefault();
                var product = _context.Product.Where(p => p.Id == productStore.Productid).SingleOrDefault();
                var store = _context.Store.Where(s => s.Id == productStore.Storeid).SingleOrDefault();
                totalCost += (int)product.Sale * qunitiy;
            }

            return cost < totalCost;

        }

        public Tuple<IEnumerable<MultiTables>, MultiTables> GetOdresByPeroidOfTime(DateTime? dateFrom,
            DateTime? dateTo)
        {
            var users = _context.Systemuser.ToList();
            var orders = _context.Userorder.ToList();

            if (dateFrom != null && dateTo != null)
            {
                orders = orders.Where(o => o.Dateoforder.Value.Date >= dateFrom.Value.Date
                                      && o.Dateoforder <= dateTo.Value.Date).ToList();
            }
            else if (dateFrom != null)
            {
                orders = orders.Where(o => o.Dateoforder.Value.Date >= dateFrom.Value.Date).ToList();
            }
            else if (dateTo != null)
            {
                orders = orders.Where(o => o.Dateoforder <= dateTo.Value.Date).ToList();
            }

            var multiTables = from user1 in users
                              join order in orders
                              on user1.Id equals order.Userid
                              select new MultiTables
                              {
                                  systemuser = user1,
                                  userorder = order
                              };

            var multiTableLoss = new MultiTables();
            foreach (var order in orders)
            {
                multiTableLoss.AddOrderloss((int)order.Id, IsLoss((int)order.Id));
            }

            return Tuple.Create<IEnumerable<MultiTables>, MultiTables>(multiTables, multiTableLoss);

        }
        public void GetTotalLose()
        {
            string totalReport = "";
            var userOrder = _context.Userorder.Where(uo => uo.IsCheckout == true).ToList();

            foreach (var order in userOrder)
            {
                var user = _context.Systemuser.Where(u => u.Id == order.Userid).SingleOrDefault();
                totalReport += user.Username + " buy ";
                int cost = (int)order.Cost;
                int totalCost = 0;

                var productOrders = _context.Productorder.Where(pr => pr.Orderid == order.Id).ToList();
                foreach (var productOrder in productOrders)
                {
                    int qunitiy = (int)productOrder.Quntity;

                    var productStore = _context.ProductStore.Where(ps => ps.Id == productOrder.Productid).SingleOrDefault();
                    var product = _context.Product.Where(p => p.Id == productStore.Productid).SingleOrDefault();
                    var store = _context.Store.Where(s => s.Id == productStore.Storeid).SingleOrDefault();
                    totalReport += qunitiy + " " + product.Namee + " from " + store.Storename;
                    totalCost += (int)product.Sale * qunitiy;
                }
                totalReport += " with Total Cost " + totalCost + " /n";
            }

        }
    }
}