using Azure;
using Azure.Data.Tables;

namespace CustomerChat.Models;

public class TableChatUser : ChatUser, ITableEntity
{
    public TableChatUser(string nick, string password, string avatarUrl) :
        base(nick, password, avatarUrl)
    {
        PartitionKey = nameof(ChatUser);
        RowKey = Guid.NewGuid().ToString();
        Timestamp = DateTimeOffset.UtcNow;
        ETag = new ETag();
    }

    public TableChatUser() : this("", "", "")
    {
    }

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
