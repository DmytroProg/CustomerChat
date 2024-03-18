using CustomerChat.Models;
using CustomerChat.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChat.Controllers;

public class ChatController : Controller
{
    private readonly IChat<TableChatMessage> _repository;

    public ChatController(IChat<TableChatMessage> repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        string? loggedNick = HttpContext.Session.GetString("LoggedNick");

        if(loggedNick is null)
        {
            return RedirectToAction("Login", "User");
        }

        var viewModel = await _repository.GetMessages();
        viewModel.Messages = viewModel.Messages
            .Where(x => x.Receiver == "All" || x.Receiver == loggedNick)
            .ToList();
        viewModel.Names.Remove(loggedNick);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(TableChatMessage message, IFormFile FileUrl)
    {
        message.Sender = HttpContext.Session.GetString("LoggedNick");
        message.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await _repository.CreateMessage(message, FileUrl);
        return RedirectToAction(nameof(Index));
    }
}
