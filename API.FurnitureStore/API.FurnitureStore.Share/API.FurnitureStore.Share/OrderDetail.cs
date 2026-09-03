using System;
using System.Collections.Generic;
using System.Text;

namespace API.FurnitureStore.Share
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
