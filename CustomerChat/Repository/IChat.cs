using CustomerChat.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChat.Repository;

public interface IChat<T>
{
    Task<ChatViewModel> GetMessages();
    Task<FileResult> DownloadFile(T message);
    Task CreateMessage(T message, IFormFile FileUrl);
}
