using System.ComponentModel.DataAnnotations;

namespace Identity.Oli.Models;

public class Customer
{
    public Guid Id { get; set; }
    
    [Required] // Name field cannot be null or empty.
    [StringLength(50, MinimumLength = 2)] // Limits length of name field.
    public string Name { get; set; }
    
    [Required]
    [EmailAddress] // Ensure valid email address
    public string Email { get; set; }
}