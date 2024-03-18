using CustomerChat.Data;
using CustomerChat.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerChat.Repository;

public class UserRepositorySql : IUser<ChatUser>
{
    private readonly CustomerChatContext _context;

    public UserRepositorySql(CustomerChatContext context)
    {
        _context = context;
    }

    public bool IsNickUnique(string nick)
    {
        return _context.ChatUser.FirstOrDefault(u => u.Nick == nick) is null;
    }

    public async Task<ChatUser?> Login(ChatUser user)
    {
        var loggedUser = await _context.ChatUser.FirstOrDefaultAsync(u => u.Nick == user.Nick &&
                                    u.Password == user.Password);
        if (loggedUser != null) {
            loggedUser.LastSeenAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            await _context.SaveChangesAsync();
        }

        return loggedUser;
    }

    public async Task<ChatUser> Register(ChatUser user, IFormFile avatar)
    {
        using var ms = new MemoryStream();
        avatar.CopyTo(ms);
        string b64 = Convert.ToBase64String(ms.ToArray());
        user.AvatarUrl = b64;
        user.JoinedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        user.LastSeenAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        await _context.ChatUser.AddAsync(user);
        _context.SaveChanges();
        return user;
    }
}
