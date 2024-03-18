using CustomerChat.Models;
using CustomerChat.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChat.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser<TableChatUser> _repository;

        public UserController(IUser<TableChatUser> repository)
        {
            _repository = repository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(TableChatUser user)
        {
            var loggedUser = await _repository.Login(user);
            if(loggedUser == null)
            {
                return RedirectToAction(nameof(Register));
            }
            else
            {
                HttpContext.Session.SetString("LoggedNick", loggedUser.Nick);
                HttpContext.Session.SetString("LoggedKey", loggedUser.RowKey);

                return RedirectToAction("Index", "Chat");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(TableChatUser user, IFormFile AvatarUrl)
        {
            if (!_repository.IsNickUnique(user.Nick))
            {
                ViewBag.Error = "Your Nick is not unique!";
                return RedirectToAction(nameof(Register));
            }

            if(AvatarUrl == null)
            {
                ViewBag.Error = "You must select avatar!";
                return RedirectToAction(nameof(Register));
            }

            await _repository.Register(user, AvatarUrl);
            return RedirectToAction(nameof(Login));
        }
    }
}
