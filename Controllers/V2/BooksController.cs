using APIFirstDemo.Helpers;
using APIFirstDemo.Models;
using APIFirstDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIFirstDemo.Controllers.V2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public BooksController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetBooks([FromQuery] QueryStringParameters bookParameters)
        { 
            var books = _repository.Book.GetBooks(bookParameters);

            var metadata = new
            {
                books.TotalCount,
                books.PageSize,
                books.CurrentPage,
                books.TotalPages,
                books.HasNext,
                books.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(books);
        }

        [HttpGet("{id}", Name = "BookById")]
        public IActionResult GetBookById(Guid id, [FromQuery] string fields)
        {
            var book = _repository.Book.GetBookById(id, fields);

            if (book == default(Entity))
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Book object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }

            _repository.Book.CreateBook(book);
            _repository.Save();

            return CreatedAtRoute("BookById", new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(Guid id, [FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Book object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }

            var dbBook = _repository.Book.GetBookById(id);
            if (dbBook == null)
            {
                return NotFound();
            }

            _repository.Book.UpdateBook(dbBook, book);
            _repository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            var book = _repository.Book.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            _repository.Book.DeleteBook(book);
            _repository.Save();

            return NoContent();
        }
    }
}
