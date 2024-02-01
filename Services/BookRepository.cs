using APIFirstDemo.DataModel;
using APIFirstDemo.Helpers;
using APIFirstDemo.Models;
using System;
using System.Linq;


namespace APIFirstDemo.Services
{
    public class BookRepository : GenericRepositoryBase<Book>, IBookRepository
    {
        private ISortHelper<Book> _sortHelper;
        private IDataShaper<Book> _dataShaper;

        public BookRepository(DemoDbContext repositoryContext,
            ISortHelper<Book> sortHelper,
            IDataShaper<Book> dataShaper)
            : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public PagedList<Entity> GetBooks(QueryStringParameters qryParameters)
        {
            var books = FindByCondition(o => o.Title.Length > 0);

            //SearchByName(ref books, qryParameters.SearchQry);

            var sortedBooks = _sortHelper.ApplySort(books, qryParameters.OrderBy);
            var shapedBooks = _dataShaper.ShapeData(sortedBooks, qryParameters.Fields);

            return PagedList<Entity>.ToPagedList(shapedBooks,
                qryParameters.PageNumber,
                qryParameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Book> books, string bookName)
        {
            if (!books.Any() || string.IsNullOrWhiteSpace(bookName))
                return;

            if (string.IsNullOrEmpty(bookName))
                return;

            books = books.Where(o => o.Title.ToLowerInvariant().Contains(bookName.Trim().ToLowerInvariant()));
        }

        public Entity GetBookById(Guid Id, string fields)
        {
            var book = FindByCondition(b => b.Id.Equals(Id))
                .DefaultIfEmpty(new Book())
                .FirstOrDefault();

            return _dataShaper.ShapeData(book, fields);
        }

        public Book GetBookById(Guid Id)
        {
            return FindByCondition(b => b.Id.Equals(Id))
                .DefaultIfEmpty(new Book())
                .FirstOrDefault();
        }

        public void CreateBook(Book book)
        {
            Create(book);
        }

        public void UpdateBook(Book dbBook, Book book)
        {
            dbBook.Title = book.Title;
            dbBook.Author = book.Author;
            dbBook.Year = book.Year;
            Update(dbBook);
        }

        public void DeleteBook(Book book)
        {
            Delete(book);
        }
    }
}
