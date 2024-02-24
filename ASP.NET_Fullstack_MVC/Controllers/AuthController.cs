using ASP.NET_Fullstack_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ASP.NET_Fullstack_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7001/api/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsync("Auth/login", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("JwtToken", tokenResponse); // Store token in session
                    return RedirectToAction("Index", "Product"); // Redirect to home page or any other page
                }
                else
                {
                    TempData["errorMessage"] = $"Invalid username or password.";
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while processing your request: {ex.Message}");
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear user's session
            HttpContext.Session.Clear();

            // Redirect to login page
            return RedirectToAction("Login", "Auth");
        }
    }
}
