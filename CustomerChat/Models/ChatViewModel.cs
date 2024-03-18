namespace CustomerChat.Models;

public class ChatViewModel
{
    public List<TableChatMessage> Messages { get; set; }
    public List<string> Names { get; set; }
    public TableChatMessage Message { get; set; }

}
