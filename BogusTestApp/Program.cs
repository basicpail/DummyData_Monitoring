using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BogusTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new SampleCustomerRepository();
            //IEnumerable<Customer> customers = repository.GetCustomers();
            var customers = repository.GetCustomers();

            Console.WriteLine(JsonConvert.SerializeObject(customers, Formatting.Indented)); 
        }
    }
}
