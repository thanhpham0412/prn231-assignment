using BookManagementClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementClient.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient client = null;
        private string CateApiUrl;

        public CategoryController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CateApiUrl = "https://localhost:44326/Category";
        }

        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync(CateApiUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();
            List<CategoryModel> items = JsonConvert.DeserializeObject<List<CategoryModel>>(strData);

            return View(items);
        }

        // GET: CategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync(CateApiUrl + "/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();
            CategoryModel item = JsonConvert.DeserializeObject<CategoryModel>(strData);


            return View(item);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            var cate = new CategoryModel
            {
                Name = collection["Name"]
            };
            var json = JsonConvert.SerializeObject(cate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.PostAsync(CateApiUrl, data);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync(CateApiUrl + "/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();
            CategoryModel item = JsonConvert.DeserializeObject<CategoryModel>(strData);


            return View(item);

        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            var cate = new CategoryModel
            {
                Id = id,
                Name = collection["Name"]
            };
            var json = JsonConvert.SerializeObject(cate);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.PutAsync(CateApiUrl, data);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync(CateApiUrl + "/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();
            CategoryModel item = JsonConvert.DeserializeObject<CategoryModel>(strData);


            return View(item);

        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.DeleteAsync(CateApiUrl + "/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["ErrorMessage"] = "There're books belong to this category. Failed to delete.";
                return RedirectToAction("Delete", "Category", new {id = id});
            }

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
