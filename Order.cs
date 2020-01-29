using System.Collections.Generic;

namespace S_CRM
{
    class Order
    {
        /// <summary>
        /// 
        /// </summary>
        public int OrderId;

        /// <summary>
        /// 
        /// </summary>
        public string DeliveryAddress;

        /// <summary>
        /// 
        /// </summary>
        public decimal TotalAmount;

        /// <summary>
        /// 
        /// </summary>
        public List<Product> ListProducts = new List<Product>();

        /// <summary>
        ///
        /// </summary>
        public void CalculateAmount()
        {
            decimal d = 0;
            foreach(Product p in ListProducts)
            {
                d = d + p.Price;
            }

            TotalAmount = d;
        }

    }
}
