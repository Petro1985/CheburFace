namespace SkillFactoryModule19.BLL.Models;

public class User
{
    public int Id { get; private init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string EMail { get; set; }
    public string? Photo { get; set; }
    public string? FavoriteMovie { get; set; }
    public string? FavoriteBook { get; set; }

    public User(
        string firstName,
        string lastName,
        string password,
        string email)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Password = password;
        this.EMail = email;
    }
}