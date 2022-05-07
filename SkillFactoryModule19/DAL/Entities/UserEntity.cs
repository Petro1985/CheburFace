namespace SkillFactoryModule19.DAL.Entities;

public class UserEntity{
    public int Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    
    public string EMail { get; set; }
    public string Photo { get; set; }
    public string FavoriteMovie { get; set; }
    public string FavoriteBook { get; set; }

    public UserEntity(int id, string firstName, string lastName, string password, string eMail, string photo, string favoriteMovie, string favoriteBook)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        EMail = eMail;
        Photo = photo;
        FavoriteMovie = favoriteMovie;
        FavoriteBook = favoriteBook;
    }
    
    public UserEntity()
    {
        
    }
}