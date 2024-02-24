using ASP.NET_Fullstack_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ASP.NET_Fullstack_MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7001/api/");
        }

        //private async Task<string> GetJwtTokenAsync()
        //{
        //    var loginUrl = "Auth/login";
        //    var loginData = new
        //    {
        //        username = "admin",
        //        password = "admin"
        //    };

        //    var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync(loginUrl, content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var tokenResponse = await response.Content.ReadAsStringAsync();
        //        return tokenResponse.Trim(); // Trim whitespace characters if necessary
        //    }
        //    else
        //    {
        //        throw new HttpRequestException($"Failed to authenticate. Status code: {response.StatusCode}");
        //    }
        //}

        private string GetOrRefreshToken()
        {
            var token = HttpContext.Session.GetString("JwtToken");

            //if (string.IsNullOrEmpty(token))
            //{
            //    token = await GetJwtTokenAsync();
            //    HttpContext.Session.SetString("JwtToken", token);
            //}

            return token;
        }

        private async Task<HttpClient> GetAuthorizedClientAsync()
        {
            var token = GetOrRefreshToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return _httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                {
                    // If JwtToken is not found in session, redirect to login page
                    return RedirectToAction("Login", "Auth");
                }

                using (var httpClient = await GetAuthorizedClientAsync())
                {
                    var response = await httpClient.GetAsync("Product");

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(data);
                        return View(productList);
                    }
                    else
                    {
                        TempData["errorMessage"] = $"Failed to retrieve products: {response.StatusCode}";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var token = GetOrRefreshToken();
            if (string.IsNullOrEmpty(token))
            {
                // If JwtToken is not found in session, redirect to login page
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            try
            {
                var token = GetOrRefreshToken();
                if (string.IsNullOrEmpty(token))
                {
                    // If JwtToken is not found in session, redirect to login page
                    return RedirectToAction("Login", "Auth");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Product", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product Created.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = $"Failed to create product: {response.StatusCode}";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = GetOrRefreshToken();
                if (string.IsNullOrEmpty(token))
                {
                    // If JwtToken is not found in session, redirect to login page
                    return RedirectToAction("Login", "Auth");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<ProductViewModel>(data);
                    return View(product);
                }
                else
                {
                    TempData["errorMessage"] = $"Failed to retrieve product details: {response.StatusCode}";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            try
            {
                var token = GetOrRefreshToken();
                if (string.IsNullOrEmpty(token))
                {
                    // If JwtToken is not found in session, redirect to login page
                    return RedirectToAction("Login", "Auth");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Product", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product details updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = $"Failed to update product details: {response.StatusCode}";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = GetOrRefreshToken();
                if (string.IsNullOrEmpty(token))
                {
                    // If JwtToken is not found in session, redirect to login page
                    return RedirectToAction("Login", "Auth");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<ProductViewModel>(data);
                    return View(product);
                }
                else
                {
                    TempData["errorMessage"] = $"Failed to retrieve product details: {response.StatusCode}";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = GetOrRefreshToken();
                if (string.IsNullOrEmpty(token))
                {
                    // If JwtToken is not found in session, redirect to login page
                    return RedirectToAction("Login", "Auth");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product deleted.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = $"Failed to delete product: {response.StatusCode}";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
                return View();
            }
        }

    }
}
