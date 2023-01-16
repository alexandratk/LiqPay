using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace lb5.MyShop
{
    public class Shop
    {
        public Shop()
        {
            Debug.WriteLine("Order.Count ==> " + Order.Count);
            if (Order.Count == 0)
            {
                for (int i = 0; i < Products.Count; i++)
                {
                    Order.Add(new Order() { CurProduct = Products[i], AmountInOrder = 0 });
                }
            }
        }

        public static List<Product> Products = new List<Product>() {
            new Product() { Id = 0, Title = "apple", Amount = 26, Price = 2 },
            new Product() { Id = 1, Title = "pineapple", Amount = 12, Price = 8 },
            new Product() { Id = 2, Title = "milk", Amount = 26, Price = 15 },
        };

        public static List<Order> Order = new List<Order>();

        public static double sum()
        {
            double sum = 0;
            for (int i = 0; i < Order.Count; i++)
            {
                sum += Order[i].AmountInOrder * Order[i].CurProduct.Price;
            }
            return sum;
        }

        public static void dicreaseAmount()
        {
            for (int i = 0; i < Order.Count; i++)
            {
              //  Order[i].CurProduct.Amount -= Order[i].AmountInOrder;
                Products[i].Amount -= Order[i].AmountInOrder;
                Order[i].AmountInOrder = 0;
            }
        }
    }
}
