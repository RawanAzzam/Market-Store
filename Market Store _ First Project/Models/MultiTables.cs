using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Store___First_Project.Models
{
    public class MultiTables
    {
        public Systemuser systemuser;
        public UserLogin userLogin;
        public Role role;

        public Store store;
        public Category category;
        public Product product;
        public ProductStore productStore;

        public ProductCategory productCategory;
        public Userorder userorder;
        public Productorder productorder;

        public Testimonial testimonial;

        public Dictionary<int, int> productRate = new Dictionary<int, int>();
        public Dictionary<int, string> orderIsLoss = new Dictionary<int, string>();

        public void AddRate(int productId , int rate)
        {
            productRate.Add(productId, rate);
        }

        public void AddOrderloss(int orderId, string loss)
        {
            orderIsLoss.Add(orderId, loss);
        }

    }
}
