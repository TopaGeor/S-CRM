namespace S_CRM
{
    class Customer
    {
        /// <summary>
        /// The orders that the customer has made
        /// </summary>
        public Order Orders = new Order();
        
        /// <summary>
        /// Customer id
        /// </summary>
        public int Id;
    }
}
