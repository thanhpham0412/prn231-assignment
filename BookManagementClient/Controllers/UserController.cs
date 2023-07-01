using BookManagementClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;

namespace BookManagementClient.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient client = null;
        private string UserApiUrl;

        public UserController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UserApiUrl = "https://localhost:44326/User";
        }

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: UserController/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: UserController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection collection)
        {
            var user = new UserModel
            {
                Username = collection["Username"],
                Password = collection["Password"]
            };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(UserApiUrl + "/login", data);

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();

                TokenDTO token = JsonConvert.DeserializeObject<TokenDTO>(strData);

                HttpContext.Session.SetString("token", token.Token);
                HttpContext.Session.SetString("role", token.Role.ToString());
                HttpContext.Session.SetString("username", user.Username);
                return RedirectToAction("Index", "Book");
            } else
            {
                TempData["ErrorMessage"] = "Invalid username or password";
                return View();
            }
            
        }

        // GET: UserController/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: UserController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(IFormCollection collection)
        {
            var user = new UserModel
            {
                Username = collection["Username"],
                Password = collection["Password"],
                Role = RoleModel.User
            };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(UserApiUrl + "/register", data);

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();

                TokenDTO token = JsonConvert.DeserializeObject<TokenDTO>(strData);
                HttpContext.Session.SetString("token", token.Token);
                HttpContext.Session.SetString("role", token.Role.ToString());
                HttpContext.Session.SetString("username", user.Username);

                return RedirectToAction("Index", "Book");
            }
            else
            {
                TempData["ErrorMessage"] = "Username is already exists.";
                return View();
            }

        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
