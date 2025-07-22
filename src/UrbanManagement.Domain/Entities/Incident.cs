using UrbanManagement.Domain.ValueObjects;

namespace UrbanManagement.Domain.Entities;

public class Incident
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Address Address { get; set; }
    public Period Period { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }

    public Incident(string title, Address address, Period period, string description, string status)
    {
        Id = Guid.NewGuid();
        Title = title ?? throw new ArgumentException("Title is required.");
        Address = address ?? throw new ArgumentException("Address is required.");
        Period = period;
        Description = description;
        Status = status;
    }
}
