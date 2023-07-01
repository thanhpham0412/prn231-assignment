using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using BusinessObject.Models;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.Authorization;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IBookRepository bookRepository;

        public CategoryController(ICategoryRepository _cateRepo, IBookRepository _bookRepo)
        {
            categoryRepository = _cateRepo;
            bookRepository = _bookRepo;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "Admin, User")]
        public IActionResult Get()
        {
            return Ok(categoryRepository.GetAll());
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetById([FromODataUri] int id)
        {
            var cate = categoryRepository.GetById(id);
            if (cate == null)
            {
                return NotFound();
            }
            return Ok(cate);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Insert([FromBody] Category cate)
        {
            categoryRepository.Insert(cate);
            categoryRepository.Save();
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] Category cate)
        {
            var curCate = categoryRepository.GetById(cate.Id);
            if (curCate == null)
            {
                return NotFound();
            }
            curCate.Name = cate.Name;

            categoryRepository.Update(curCate);
            categoryRepository.Save();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var cate = categoryRepository.GetById(id);
            if (cate == null)
            {
                return NotFound();
            }
            if (bookRepository.ContainCategory(id))
            {
                return BadRequest("There're books belong to this category. Failed to delete.");
            }

            categoryRepository.Delete(cate);
            categoryRepository.Save();
            return Ok();
        }
    }
}
