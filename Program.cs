using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S_CRM
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = "products.csv";
            var productdictionary = new Dictionary<string, Product>(); // key is the id of the product, value is the products
            var descriptionprices = new Dictionary<string, decimal>(); // key is the description of a product, value is the price of it use it for pricing
            //Question 1, 2 and 3
            try
            {
                var linenumber = 1;
                foreach (var line in File.ReadAllLines(filename))
                {
                    /*
                    * Cheking if we have the only one ; in a line
                    * Otherwise return
                    */
                   if(!CheckLine(line, linenumber))
                    {
                        return;
                    }

                    /*
                    * Split at `;`, if any of the spliting parties is empty write a message and return 
                    */
                    var splitedline = line.Split(';');
                    if (string.IsNullOrWhiteSpace(splitedline[0]) ||
                        string.IsNullOrWhiteSpace(splitedline[1]))
                    {
                        WriteMessages(new string[]
                        {
                            $"The product at line {linenumber}", 
                            "Has empty id or description", 
                            "Exiting system"
                        });
                        return;
                    }

                    /*
                     * Create a new product
                     */
                    Product newproduct = new Product()
                    {
                        ProductId = splitedline[0],
                        Description = splitedline[1]
                    };

                    /*
                        * Try to add a product in Dictionary according to the key value
                        * If it fails a set of messages is written and return
                    */
                    //Question 3
                    if (productdictionary.TryAdd(newproduct.ProductId, newproduct))
                    {
                        /*
                        * When we met a new description generate generate a price for the product and save it
                        * If it not then find the first key description and assign to the new product
                        */
                        //Question 2
                        if (descriptionprices.ContainsKey(newproduct.Description))
                        {
                            var temp = productdictionary.First(
                                s => s.Value.Description.Equals(newproduct.Description));
                            newproduct.SetPrice(temp.Value.Price);
                        }
                        else
                        {
                            newproduct.GeneratePrice();
                            descriptionprices.Add(newproduct.Description, newproduct.Price);
                        }
                    }
                    else
                    {
                        WriteMessages(new string[]
                        {
                            @$"Faild to add product with id {newproduct.ProductId}",
                            "Propably a duplicate id", 
                            "Exiting system"
                        });
                        return;
                    }

                    linenumber++; // keep track of the lines
                }
            }
            catch (Exception e)
            {
                WriteMessages(new string[] 
                {
                    e.Message 
                });
                return;
            }

            //Question 4
            var customer0 = new Customer()
            {
                Id = 0
            };
            var customer1 = new Customer()
            {
                Id = 1
            };
            CreateOrder(ref customer0, productdictionary);
            CreateOrder(ref customer1, productdictionary);

            //Question 5a) 
            customer0.Orders.CalculateAmount();
            customer1.Orders.CalculateAmount();
            BestCustomer(customer0.Orders.TotalAmount, customer1.Orders.TotalAmount,
                         customer0.Id, customer1.Id);

            //Question 5b)
            var solddictionaries = new Dictionary<string, int>(); // key is the id, value how many
            PopulateSoldItems(ref solddictionaries, customer0.Orders.ListProducts);
            PopulateSoldItems(ref solddictionaries, customer1.Orders.ListProducts);

            var orderlist = solddictionaries.ToList();
            orderlist.Sort((left, right) => left.Value.CompareTo(right.Value));
            orderlist.Reverse();
            Console.WriteLine("The 10 most sold items are:");
            if (orderlist.Count <= 10)
            {
                WriteBestProducts(orderlist);
            }
            else
            {
                WriteBestProducts(orderlist.Take(10).ToList());
            }    
        }

        /// <summary>
        /// Check if a line has only one `;`and returns true if that is the case, otherwise is false
        /// If we do not have only one `;` then a set of messages will be written at the Console with the function WriteMessages
        /// </summary>
        /// <param name="line"> The content of the line </param>
        /// <param name="linenumber"> The line number at .csv </param>
        /// <returns></returns>
        static bool CheckLine(string line, int linenumber)
        {
            try
            {
                line.SingleOrDefault(s => s.Equals(';'));
            }
            catch (ArgumentNullException e)
            {
                WriteMessages(new string[]
                {
                                e.Message,
                                $"In line {linenumber} no `;` were found",
                                "Exiting system"
                });
                return false;
            }
            catch (InvalidOperationException e)
            {
                WriteMessages(new string[]
                {
                                e.Message,
                                $"In line {linenumber} more than one `;` found",
                                "Exiting system"
                });
                return false;
            }
            return true;
        }

        /// <summary>
        /// Prints 1 or more strings into the Console
        /// </summary>
        /// <param name="output"></param>
        static void WriteMessages(string[] output)
        {
            foreach (var s in output)
            {
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// For a customer populate his ProductList with random products
        /// </summary>
        /// <param name="c">Pass a Customer with reference </param>
        /// <param name="dic"> Dictionary with id and products</param>
        static void CreateOrder(ref Customer c, Dictionary<string, Product> dic)
        {
            var rand = new Random();
            var size = dic.Keys.Count;
            var k = dic.Keys.ElementAt(rand.Next(size));
            for (var i = 0; i < 10; i++)
            {
                c.Orders.ListProducts.Add(dic[k]);
                k = dic.Keys.ElementAt(rand.Next(size));
            }
        }

        /// <summary>
        /// Compare two customers expensives and print how has spend the most, if they spend the same prints apropiet message
        /// </summary>
        /// <param name="amount0">expensives of a customer </param>
        /// <param name="amount1">expensives of another customer </param>
        /// <param name="id0">id of a customer </param>
        /// <param name="id1">id of another customer </param>
        static void BestCustomer(decimal amount0, decimal amount1, int id0, int id1)
        {
            if (amount0 < amount1)
            {
                WriteMessages(new string[]
                {
                    $"The customer with {id1} ",
                    "is the most valuable customer ",
                    $"with total expensives {amount1}"
                });
            }
            else if (amount0 > amount1)
            {
                WriteMessages(new string[]
                {
                    $"The customer with id {id0} ",
                    "is the most valuable customer ",
                    $"with total expensives {amount0}"
                });
            }
            else
            {
                WriteMessages(new string[]
                {
                    "Both customers are valuable"
                });
            }
        }

        /// <summary>
        /// The dictionary should have how many times a product is sold
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="products"></param>
        static void PopulateSoldItems(ref Dictionary<string, int> dic, List<Product> products)
        {
            foreach (var p in products)
            {
                if (dic.ContainsKey(p.ProductId))
                {
                    dic[p.ProductId]++;
                }
                else
                {
                    dic[p.ProductId] = 1;
                }
            }
        }

        /// <summary>
        /// Accepts a list that was a dictionary and prints it
        /// </summary>
        /// <param name="list"></param>
        static void WriteBestProducts(List<KeyValuePair<string, int>> list)
        {
            foreach (var l in list)
            {
                WriteMessages(new string[]
                {
                    $"The item with id {l.Key} ",
                    $"and with amount sold {l.Value}"
                });
            }
        }
    }
}
