namespace CustomerChat.Models;

public class ChatUser
{
    public ChatUser(string nick, string password, string avatarUrl)
    {
        Nick = nick;
        Password = password;
        AvatarUrl = avatarUrl;
        JoinedAt = DateTime.UtcNow;
        LastSeenAt = DateTime.UtcNow;
    }

    public ChatUser() : this("", "", "")
    {
    }

    public int Id { get; set; }
    public string Nick { get; set; }
    public string Password { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime LastSeenAt { get; set; }
    public string AvatarUrl { get; set; }
}
