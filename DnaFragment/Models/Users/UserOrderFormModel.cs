namespace DnaFragment.Models.Users
{
    using System.Collections.Generic;
    using DnaFragment.Models.LrProducts;

    public class UserOrderFormModel
    {           
        public string FullName{ get; set; }
       
        public string ShippingAddress { get; init; }
     
        public string City { get; init; }
       
        public string PhoneNumber { get; init; }

        public int DeliveryNumber { get; init; }

        public decimal Total { get; init; }        

        public string BagProducts { get; set; }

        public int BagId { get; init; }
    }
}
