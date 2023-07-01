using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using BusinessObject.Models;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using BookManagementAPI.DTO;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository bookRepository;

        public BookController(IBookRepository _repo)
        {
            bookRepository = _repo;
        }

        [HttpGet]
        [EnableQuery(PageSize = 3)]
        [Authorize(Roles = "Admin, User")]
        public IActionResult Get()
        {
            return Ok(bookRepository.GetAll());
        }

        [HttpGet("pageInfo")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetPageInfo([FromQuery] int? page, [FromQuery] string? search)
        {
            var searchStr = search ?? "";
            var books = bookRepository.GetAll().FindAll(b => b.Title.Contains(searchStr));
            var totalPage = Convert.ToInt32(Math.Ceiling((decimal)books.Count / 3M));
            var currentPage = page ?? 1;
            var query = "&$skip=" + (currentPage - 1) * 3;
            return Ok(new PageInfoDTO
            {
                TotalPage = totalPage,
                CurrentPage = currentPage,
                PageQuery = query
            });
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetById([FromODataUri] int id)
        {
            var book = bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Insert([FromBody] Book book)
        {
            if (bookRepository.GetByISBN(book.ISBN) != null)
            {
                return BadRequest("ISBN already exists.");
            }
            bookRepository.Insert(book);
            bookRepository.Save();
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] Book book)
        {
            var curBook = bookRepository.GetById(book.Id);
            if (curBook == null)
            {
                return NotFound();
            }
            curBook.ISBN = book.ISBN;
            curBook.Title = book.Title;
            curBook.Author = book.Author;
            curBook.CategoryId = book.CategoryId;

            bookRepository.Update(curBook);
            bookRepository.Save();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var book = bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            bookRepository.Delete(book);
            bookRepository.Save();
            return Ok();
        }

    }
}
