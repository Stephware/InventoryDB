using InventoryDB.Models.Database;
using InventoryDB.Repository.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        if (_userRepository.UsernameExists(username))
        {
            ViewBag.Error = "Username already exists";
            return View();
        }

        var user = new User
        {
            Username = username,
            PasswordHash = HashPassword(password)
        };

        _userRepository.AddUser(user);

        TempData["Success"] = "Registration successful! Please log in.";
        return RedirectToAction("Login");
    }
    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var passwordHash = HashPassword(password);
        var user = _userRepository.ValidateUser(username, passwordHash);

        if (user == null)
        {
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Username", user.Username);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        TempData["Success"] = "Welcome back!";

        return RedirectToAction("Index", "Inventory");
    }

    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();    

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", "Account");
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
}