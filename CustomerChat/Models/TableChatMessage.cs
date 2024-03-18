using Azure;
using Azure.Data.Tables;

namespace CustomerChat.Models;

public class TableChatMessage : ChatMessage, ITableEntity
{
    public TableChatMessage(string sender, string receiver, string text, string? fileUrl)
        : base(sender, receiver, text, fileUrl)
    {
        PartitionKey = nameof(ChatMessage);
        RowKey = Guid.NewGuid().ToString();
        Timestamp = DateTimeOffset.UtcNow;
        ETag = new ETag();
    }

    public TableChatMessage() : this("", "", "", null)
    {
    }

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
