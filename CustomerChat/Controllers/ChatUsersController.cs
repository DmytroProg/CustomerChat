using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerChat.Models;
using CustomerChat.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChat.Controllers
{
    public class ChatUsersController : Controller
    {
        private readonly IUser<ChatUser> _repository;

        public ChatUsersController(IUser<ChatUser> repository)
        {
            _repository = repository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(ChatUser user)
        {
            var loggedUser = await _repository.Login(user);
            if (loggedUser == null)
            {
                return RedirectToAction(nameof(Register));
            }
            else
            {
                HttpContext.Session.SetString("LoggedNick", loggedUser.Nick);
                HttpContext.Session.SetInt32("LoggedKey", loggedUser.Id);

                return RedirectToAction("Index", "Chat");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ChatUser user, IFormFile AvatarUrl)
        {
            if (!_repository.IsNickUnique(user.Nick))
            {
                ViewBag.Error = "Your Nick is not unique!";
                return RedirectToAction(nameof(Register));
            }

            if (AvatarUrl == null)
            {
                ViewBag.Error = "You must select avatar!";
                return RedirectToAction(nameof(Register));
            }

            await _repository.Register(user, AvatarUrl);
            return RedirectToAction(nameof(Login));
        }
    }
}
