namespace FitnessApp.API.Models;

public class FitnessAppUser
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string City { get; set; }

    public FitnessAppUser(int userId, string userName, string firstName, string lastName, DateOnly? dateOfBirth, string city)
    {
        UserId = userId;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        City = city;
    }
}
