using BookManagementClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;

namespace BookManagementClient.Controllers
{
    public class BookController : Controller
    {
        private readonly HttpClient client = null;
        private string BookApiUrl;
        private string CateApiUrl;
        private string adminToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2ODgxMjc5NDAsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.0aWJsBD6n3f0XvZtt8ssN5A-0vpX64r0eE8Qxq0eof8";
        private string userToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InVzZXIxIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiVXNlciIsImV4cCI6MTY4ODAzMjczNCwiaXNzIjoiVGVzdC5jb20iLCJhdWQiOiJUZXN0LmNvbSJ9.0iHNV8nDkmftO-5GDTEUv5jLLYMQhcsZWrgyv5Dnc_Q";


        public BookController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BookApiUrl = "https://localhost:44326/Book";
            CateApiUrl = "https://localhost:44326/Category";
        }

        // GET: BookController
        public async Task<IActionResult> Index(int? page, string? search)
        {
            var _search = search ?? "";
            var _page = page ?? 1;
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync(BookApiUrl + "/pageInfo?page=" + _page + "&search=" + _search);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            } else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();
            PageInfoDTO pageInfo = JsonConvert.DeserializeObject<PageInfoDTO>(strData);

            response = await client.GetAsync(BookApiUrl + "?$expand=Category&$filter=contains(Title,'" + search + "')" + pageInfo.PageQuery);
            strData = await response.Content.ReadAsStringAsync();
            List<BookModel> items = JsonConvert.DeserializeObject<List<BookModel>>(strData);

            TempData["search"] = _search;
            TempData["pageInfo"] = pageInfo;
            return View(items);
        }

        // GET: BookController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.GetAsync(BookApiUrl + "/" + id + "?$expand=Category");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();

            BookModel item = JsonConvert.DeserializeObject<BookModel>(strData);

            return View(item);
        }

        // GET: BookController/Create
        public async Task<IActionResult> Create()
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
            List<CategoryModel> cateList = JsonConvert.DeserializeObject<List<CategoryModel>>(strData);
            var items = cateList.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            ViewBag.CateList = new SelectList(items, "Value", "Text");
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            var book = new BookModel
            {
                ISBN = collection["ISBN"],
                Title = collection["Title"],
                Author = collection["Author"],
                CategoryId = Convert.ToInt32(collection["Category"])
            };
            var json = JsonConvert.SerializeObject(book);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(BookApiUrl, data);
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

        // GET: BookController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.GetAsync(BookApiUrl + "/" + id + "?$expand=Category");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();

            BookModel item = JsonConvert.DeserializeObject<BookModel>(strData);

            return View(item);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            var updateBook = new BookModel
            {
                Id = id,
                ISBN = collection["ISBN"],
                Title = collection["Title"],
                Author = collection["Author"],
                CategoryId = Convert.ToInt32(collection["CategoryId"])
            };
            var json = JsonConvert.SerializeObject(updateBook);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(BookApiUrl, data);
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

        // GET: BookController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.GetAsync(BookApiUrl + "/" + id + "?$expand=Category");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("UnauthorizedError", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            string strData = await response.Content.ReadAsStringAsync();

            BookModel item = JsonConvert.DeserializeObject<BookModel>(strData);

            return View(item);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.DeleteAsync(BookApiUrl + "/" + id);
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
    }
}
