using Identity.Oli.Models;

namespace Identity.Oli.Data;

// Static class meaning it belongs to the class itself and not an instance.
// All parts of the app share the same instance of this class and its data.
public static class CustomerRepository 
{
        // Static list to stores customer data in memory during apps lifetime.
        // Shared resource that can be accessed without instantiation.
        public static List<Customer> Customers = new List<Customer>();
}