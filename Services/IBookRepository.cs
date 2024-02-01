using APIFirstDemo.Helpers;
using APIFirstDemo.Models;

namespace APIFirstDemo.Services
{
    public interface IBookRepository : IGenericRepositoryBase<Book>
    {
        PagedList<Entity> GetBooks(QueryStringParameters qryParameters);
        Entity GetBookById(Guid Id, string fields);
        Book GetBookById(Guid Id);
        void CreateBook(Book book);
        void UpdateBook(Book dbBook, Book book);
        void DeleteBook(Book book);
    }
}
