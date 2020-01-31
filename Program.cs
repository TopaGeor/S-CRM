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
            var productdictionary = new Dictionary<string, Product>();
            var descriptionprices = new Dictionary<string, decimal>();
            //Question 1, 2 and 3
            try
            {
                var file = File.ReadAllText(filename);
                var line = "";
                var linenumber = 1;
                foreach (var f in file)
                {
                    if (f == '\n')
                    {
                        /*
                         * Cheking if we have the only one ; in a line
                         * Otherwise exception
                         */
                        if(!CheckLine(line, linenumber))
                        {
                            return;
                        }

                        /*
                         * Split on ;
                         */
                        var splitedline = line.Split(';');
                        if (string.IsNullOrWhiteSpace(splitedline[0]) ||
                            string.IsNullOrWhiteSpace(splitedline[1]))
                        {
                            WriteMessages(new string[]
                                {$"The product at line {linenumber}", "Has empty id or description", "Exiting system"});
                            return;
                        }

                        Product newproduct = new Product()
                        {
                            ProductId = splitedline[0],
                            Description = splitedline[1]
                        };

                        /*
                         * Try to add a product in Dictionary
                         * Otherwise exception
                         * Dictionary has to have unique key values
                         */
                        //Question 3
                        if (productdictionary.TryAdd(newproduct.ProductId, newproduct))
                        {
                            /*
                             * Generate a value for the product according the description
                             * For the first time
                             * Otherwise find the price of the description and set it to 
                             * The product
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

                        line = "";// clears variable so new line can be written
                        linenumber++;
                    }
                    else
                    {
                        line += f;
                    }
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
            CreateOrders(ref customer0, ref customer1, productdictionary);

            //Question 5a) 
            customer0.Orders.CalculateAmount();
            customer1.Orders.CalculateAmount();
            BestCustomer(customer0.Orders.TotalAmount, customer1.Orders.TotalAmount,
                customer0.Id, customer1.Id);

            //Question 5b)
            /*
             * key is the id, value how many
             */
            var solddictionaries = new Dictionary<string, int>();
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

        static void WriteMessages(string[] output)
        {
            foreach (var s in output)
            {
                Console.WriteLine(s);
            }
        }

        static void CreateOrders(ref Customer c0, ref Customer c1, Dictionary<string, Product> dic)
        {
            var rand = new Random();
            var size = dic.Keys.Count;
            var k = dic.Keys.ElementAt(rand.Next(size));
            for (var i = 0; i < 10; i++)
            {
                c0.Orders.ListProducts.Add(dic[k]);
                k = dic.Keys.ElementAt(rand.Next(size));
                c1.Orders.ListProducts.Add(dic[k]);
                k = dic.Keys.ElementAt(rand.Next(size));
            }
        }

        static void BestCustomer(decimal amount0, decimal amount1, int id0, int id1)
        {
            if (amount0 < amount1)
            {
                Console.WriteLine($"The customer with {id1} "
                                  + $"is the most valuable customer "
                                  + $"with total expensives {amount1}");
            }
            else if (amount0 > amount1)
            {
                Console.WriteLine($"The customer with id {id0} "
                                  + $"is the most valuable customer "
                                  + $"with total expensives {amount0}");
            }
            else
            {
                Console.WriteLine("Both customers are valuable");
            }

        }

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

        static void WriteBestProducts(List<KeyValuePair<string, int>> list)
        {
            foreach (var l in list)
            {
                Console.WriteLine($"The item with id {l.Key} "
                                  + $"and with amount sold {l.Value}");
            }
        }
    }
}
