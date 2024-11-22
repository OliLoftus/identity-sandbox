using Identity.Oli.Models;
using MongoDB.Driver; // Import to interact with db

namespace Identity.Oli.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    
    // Constructor: Initializes the db connection using config settings.
    public MongoDbContext(IConfiguration configuration)
    {
        // Create a MongoClient instance with connection string from appsetting.json.
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        
        // Access db by name
        _database = client.GetDatabase(configuration["DatabaseName"]);
    }
    
    // Property to access the "Customers" collection in the db
    public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customers");
}