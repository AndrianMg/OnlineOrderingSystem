using OnlineOrderingSystem.Models;
using System.Linq;

namespace OnlineOrderingSystem.Data.Seeders
{
    public static class CustomerSeeder
    {
        public static void Seed(OrderingDbContext context)
        {
            if (!context.Customers.Any())
            {
                var sampleCustomers = new[]
                {
                    new Customer { FirstName = "John", LastName = "Doe", Email = "john@example.com", Address = "123 Main St, London, UK", PreferredPaymentMethod = "Credit" },
                    new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Address = "456 Oak Ave, Manchester, UK", PreferredPaymentMethod = "Cash" },
                    new Customer { FirstName = "Mike", LastName = "Johnson", Email = "mike@example.com", Address = "789 Pine Rd, Birmingham, UK", PreferredPaymentMethod = "Credit" }
                };

                context.Customers.AddRange(sampleCustomers);
            }
        }
    }
}