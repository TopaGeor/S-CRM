using System.Collections.Generic;

namespace S_CRM
{
    class Order
    {
        /// <summary>
        /// The id of the order
        /// </summary>
        public int OrderId;

        /// <summary>
        /// Where to deliver the order
        /// </summary>
        public string DeliveryAddress;

        /// <summary>
        /// The total cost of the order
        /// </summary>
        public decimal TotalAmount;

        /// <summary>
        /// Order products
        /// </summary>
        public List<Product> ListProducts = new List<Product>();

        /// <summary>
        ///  Calculates the cost of the order acourding to the products
        ///  that are inside the list and saves it at TotalAmount
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
