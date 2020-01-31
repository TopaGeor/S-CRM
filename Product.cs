using System;

namespace S_CRM
{
    class Product
    {
        /// <summary>
        /// 
        /// </summary>
        readonly Random rand = new Random();

        /// <summary>
        /// 
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void GeneratePrice()
        {
            Price = rand.Next(1, 100);
            Price += (decimal)rand.NextDouble();
            Price = decimal.Truncate(Price * 100) / 100;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public void SetPrice(decimal d)
        {
            Price = d;
        }
    }
}
