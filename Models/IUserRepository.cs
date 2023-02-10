namespace authentication.Models
{
    public interface IUserRepository
    {
        User authenticate(string username, string password);
        User getById(int id);
        IEnumerable<User> getAll();
        string generateToken(int id);
    }
}