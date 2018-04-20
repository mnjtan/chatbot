using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Client.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ChatBot.Client.Controllers
{
    public class HomeController : Controller
    {
        const string SessionKeyName = "_Name";
        const string SessionKeyEmail = "_Email";

        public IActionResult Index()
        {
            //if signed in
            if (HttpContext.Session.GetString(SessionKeyEmail) != null)
            {
                var name = HttpContext.Session.GetString(SessionKeyName);
                ViewBag.LoggedIn = "true";
                return RedirectToAction("Bot");
            }
            //redirects to sign in, or continue as guest
            return RedirectToAction("SignIn");
        }

        public IActionResult About()
        {
            //if signed in
            if (HttpContext.Session.GetString(SessionKeyEmail) != null)
            {
                var name = HttpContext.Session.GetString(SessionKeyName);
                ViewBag.LoggedIn = "true";
            }
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Bot()
        {
            //if signed in
            if (HttpContext.Session.GetString(SessionKeyEmail) != null)
            {
                var name = HttpContext.Session.GetString(SessionKeyName);
                ViewBag.LoggedIn = "true";
                ViewBag.Message = "Welcome " + name;
            }
            else
            {
                ViewBag.Message = "Welcome Guest";
            }

            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new SignInViewModel());
        }

        [HttpPost]
        public IActionResult SignIn(SignInViewModel model)
        {
            //attempting to find customer in database
            User user = DataReader.SignIn(model.Email).GetAwaiter().GetResult();

            //if not found, show error message
            if (user == null)
            {
                ViewBag.Error = "We do not recognize your email and/or password. Please try again or Register for an account.";
                return View(model);
            }

            //if found, save to session and redirect to UserHome
            HttpContext.Session.SetString(SessionKeyName, user.Name);
            HttpContext.Session.SetString(SessionKeyEmail, user.Email);

            return RedirectToAction("Bot");

        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            //try to register customer
            var newUser = DataReader.RegisterUser(model).GetAwaiter().GetResult();

            if (newUser == null)
            {
                ViewBag.Error = "An account already exists for that email!";
                return View(model);
            }

            //signin
            User user = DataReader.SignIn(model.Email).GetAwaiter().GetResult();

            //if not found, show error message
            if (user == null)
            {
                ViewBag.Error = "We do not recognize your email and/or password. Please try again or Register for an account.";
                return RedirectToAction("SignIn");
            }

            //if found, save to session and redirect to UserHome
            HttpContext.Session.SetString(SessionKeyName, user.Name);
            HttpContext.Session.SetString(SessionKeyEmail, user.Email);

            return RedirectToAction("Bot");
        }

    }

    //Extension class to simplify storing objects in session state
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

}
