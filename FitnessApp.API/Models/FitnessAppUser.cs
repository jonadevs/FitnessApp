namespace FitnessApp.API.Models;

public class FitnessAppUser(int userId, string userName, string firstName, string lastName, DateOnly? dateOfBirth, string city)
{
    public int UserId { get; set; } = userId;

    public string UserName { get; set; } = userName;

    public string FirstName { get; set; } = firstName;

    public string LastName { get; set; } = lastName;

    public DateOnly? DateOfBirth { get; set; } = dateOfBirth;

    public string City { get; set; } = city;
}
