namespace CustomerChat.Repository;

public interface IUser<T>
{
    Task<T?> Login(T user);
    Task<T> Register(T user, IFormFile avatar);
    bool IsNickUnique(string nick);
}
