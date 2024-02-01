namespace Entities;
public class CustomerDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? SSN { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? HomeStreet { get; set; }
    public string? HomeCity { get; set; }
    public string? HomeState { get; set; }
    public string? HomeZip { get; set; }
    public string? OfficeStreet { get; set; }
    public string? OfficeCity { get; set; }
    public string? OfficeState { get; set; }
    public string? OfficeZip { get; set; }
    public string? Title { get; set; }
    public string? Salary { get; set; }
    public DateTime DateImported { get; set; }
}
