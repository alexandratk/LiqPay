using System.Collections.Generic;
using XAct.Messages;

namespace lb5.MyShop
{
    public class Order
    {
        public Product CurProduct { get; set; }
        public int AmountInOrder { get; set; }
    }
}