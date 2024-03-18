namespace CustomerChat.Models;

public class ChatMessage
{
    public ChatMessage(string sender, string receiver, string text, string? fileUrl)
    {
        Sender = sender;
        Receiver = receiver;
        MessageText = text;
        FileUrl = fileUrl;
    }

    public ChatMessage() : this("", "", "", null)
    {
    }

    public int Id { get; set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public string MessageText { get; set; }
    public string? FileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
