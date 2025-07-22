namespace UrbanManagement.Domain.ValueObjects;

public class Address
{
    public string Street { get; set; }
    public int Number { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public ZipCode ZipCode { get; set; }

    public Address(string street, int number, string neighborhood, string city, ZipCode zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street is required.");
        if (int.IsNegative(number))
            throw new ArgumentException("Number is can't negative.");
        if (string.IsNullOrWhiteSpace(neighborhood))
            throw new ArgumentException("Neighborhood is required.");
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.");
        ArgumentNullException.ThrowIfNull(zipCode);

        Street = street;
        Number = number;
        Neighborhood = neighborhood;
        City = city;
        ZipCode = zipCode;
    }

    public override bool Equals(object? obj)
    {
        return obj is Address other &&
        Street == other.Street &&
        Number == other.Number &&
        Neighborhood == other.Neighborhood &&
        City == other.City &&
        ZipCode.Equals(other.ZipCode);
    }

    public override int GetHashCode() =>
        HashCode.Combine(Street, Number, Neighborhood, City, ZipCode);

    public override string ToString() =>
        $"{Street}, {Number} - {Neighborhood}, {City} - {ZipCode}";
}
