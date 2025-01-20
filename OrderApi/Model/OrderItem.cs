﻿namespace OrderApi.Model
{

    public class OrderItem
    {

        public int? Id { get; set; }
        
        public Order? Order { get; set; }

        public int? ProductId { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

    }

}
