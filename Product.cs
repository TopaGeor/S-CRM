using System;

namespace S_CRM
{
    class Product
    {
        /// <summary>
        /// A supporting variable for creating random numbers
        /// </summary>
        readonly Random rand = new Random();

        /// <summary>
        /// Product ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Description of the Product
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The price of the product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// When a new product is found out a random price is calculated between 1 and 100
        /// </summary>
        public void GeneratePrice()
        {
            Price = rand.Next(1, 100);
            Price += (decimal)rand.NextDouble();
            Price = decimal.Truncate(Price * 100) / 100;
        }

        /// <summary>
        /// Set the price of the product
        /// </summary>
        /// <param name="d"></param>
        public void SetPrice(decimal d)
        {
            Price = d;
        }
    }
}
