using APIFirstDemo.Models;
using APIFirstDemo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIFirstDemo.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BooksController : ControllerBase
    {
        //https://github.com/Microsoft/api-guidelines/blob/vNext/Guidelines.md#971-filter-operations
        //https://code-maze.com/data-shaping-aspnet-core-webapi/
        private readonly List<Book> _books;
        public BooksController()
        {
            // Initialize with some dummy data
            _books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book 1", Author = "Author 1", Year = 2020 },
                new Book { Id = Guid.NewGuid(), Title = "Book 2", Author = "Author 2", Year = 2021 },
                new Book { Id = Guid.NewGuid(), Title = "Book 3", Author = "Author 3", Year = 2013 },
                new Book { Id = Guid.NewGuid(), Title = "Book 4", Author = "Author 4", Year = 2014 },
                new Book { Id = Guid.NewGuid(), Title = "Book 5", Author = "Author 5", Year = 2015 },
                new Book { Id = Guid.NewGuid(), Title = "Book 6", Author = "Author 6", Year = 2016 },
                new Book { Id = Guid.NewGuid(), Title = "Book 7", Author = "Author 7", Year = 2017 },
                new Book { Id = Guid.NewGuid(), Title = "Book 8", Author = "Author 8", Year = 2018 },
                new Book { Id = Guid.NewGuid(), Title = "Book 9", Author = "Author 9", Year = 2019 },
                new Book { Id = Guid.NewGuid(), Title = "Book 10", Author = "Author 10", Year = 2010 },
                new Book { Id = Guid.NewGuid(), Title = "Book 11", Author = "Author 11", Year = 2011 },
                new Book { Id = Guid.NewGuid(), Title = "Book 12", Author = "Author 12", Year = 2012 },
                new Book { Id = Guid.NewGuid(), Title = "Book 13", Author = "Author 13", Year = 2013 },
                new Book { Id = Guid.NewGuid(), Title = "Book 14", Author = "Author 14", Year = 2014 },
                new Book { Id = Guid.NewGuid(), Title = "Book 15", Author = "Author 15", Year = 2015 },
                new Book { Id = Guid.NewGuid(), Title = "Book 16", Author = "Author 16", Year = 2016 },
                new Book { Id = Guid.NewGuid(), Title = "Book 17", Author = "Author 17", Year = 2017 },
                new Book { Id = Guid.NewGuid(), Title = "Book 18", Author = "Author 18", Year = 2018 },
                new Book { Id = Guid.NewGuid(), Title = "Book 19", Author = "Author 19", Year = 2019 },
                new Book { Id = Guid.NewGuid(), Title = "Book 20", Author = "Author 20", Year = 2020 },
                new Book { Id = Guid.NewGuid(), Title = "Book 21", Author = "Author 21", Year = 2021 },
                new Book { Id = Guid.NewGuid(), Title = "Book 22", Author = "Author 22", Year = 2022 },
                new Book { Id = Guid.NewGuid(), Title = "Book 23", Author = "Author 23", Year = 2013 },
                new Book { Id = Guid.NewGuid(), Title = "Book 24", Author = "Author 24", Year = 2014 },
                new Book { Id = Guid.NewGuid(), Title = "Book 25", Author = "Author 25", Year = 2015 }
            };
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            return Ok(_books);
        }

        [HttpPost]
        public ActionResult<Book> CreateBook(BookInputModel bookInput)
        {
            var newBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = bookInput.Title,
                Author = bookInput.Author,
                Year = bookInput.Year
            };

            _books.Add(newBook);

            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBookById(Guid id)
        {
            var book = _books.Find(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPut("{id}")]
        public ActionResult<Book> UpdateBook(Guid id, BookInputModel bookInput)
        {
            var book = _books.Find(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            book.Title = bookInput.Title;
            book.Author = bookInput.Author;
            book.Year = bookInput.Year;

            return Ok(book);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            var book = _books.Find(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            _books.Remove(book);

            return NoContent();
        }
    }
}
