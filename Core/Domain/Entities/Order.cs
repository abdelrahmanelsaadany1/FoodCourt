﻿using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order:BaseEntity
    {
        public enum OrderStatus { Pending, Accepted, Rejected, Paid, InTransit, Delivered }


        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal PlatformFee { get; set; }
        public double DistanceKm { get; set; }
        

        public string ClientId { get; set; }
        public virtual User Client { get; set; }

        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
        public virtual Payment Payment { get; set; }

    }
}
