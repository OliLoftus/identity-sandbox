using Identity.Oli.Data;
using Identity.Oli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Identity.Oli.Controllers;

// Tells ASP.NET core that this class contains endpoints.
[ApiController]
// The base route is /customer.
[Route("customer")]
public class CustomerController : ControllerBase
{
    // GET: /customer/{id}
    // This endpoint gets a customer by their unique id
    [HttpGet("{id}")]
    public IActionResult GetCustomer(Guid id)
    {
        // Uses LINQ's FirstOrDefault to find the first match or return null if none is found.
        var customer = CustomerRepository.Customers.FirstOrDefault(customer => customer.Id == id);
        if (customer == null)
            return NotFound();
        // If found returns 200 OK response with customer's data
        return Ok(customer);
    }
    
    // POST: /customer
    // This endpoint is used to create a new customer.
    // Accepting a JSON object representing the customer in the request body.
    [HttpPost]
    public IActionResult CreateCustomer([FromBody] Customer newCustomer)
    {
        // Generates GUID for customer ID.
        newCustomer.Id = Guid.NewGuid();
        // Adds customer to Customers list.
        CustomerRepository.Customers.Add(newCustomer);
        
        // Returns a 201 Created response with the URI of new customer.
        return CreatedAtAction(
            nameof(GetCustomer), // The action name to generate the URI for the new resource.
            new { id = newCustomer.Id }, // Route parameters
            newCustomer// The newly created customer is included in the response body.
            );
    }
    
    // PUT: /customer/{id}
    // This endpoint is used to update a customer
    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(Guid id, [FromBody] Customer updatedCustomer)
    {
        // Searches repository for existing customer by id.
        var customer = CustomerRepository.Customers.FirstOrDefault(customer => customer.Id == id);
        
        // Returns 404 Not Found if the customer doesn't exist
        if (customer == null)
            return NotFound();
        
        // Updates customer with new values
        customer.Name = updatedCustomer.Name;
        customer.Email = updatedCustomer.Email;
        
        // Returns 204 No Content to indicate update was successful
        return NoContent();
    }
}