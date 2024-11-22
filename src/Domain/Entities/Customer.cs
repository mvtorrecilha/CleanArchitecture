using Domain.Abstractions;

namespace Domain.Entities;

public class Customer : Entity
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public bool IsActive { get; set; }

    public Customer(Guid Id, string Name, string Email, DateTime BirthDate, bool IsActive)
    {
        this.Id = Id;
        this.Name = Name;
        this.Email = Email;
        this.BirthDate = BirthDate;
        this.IsActive = IsActive;
    }

    public Customer()
    {
            
    }

}