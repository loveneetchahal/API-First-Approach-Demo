using APIFirstDemo.DataModel;
using APIFirstDemo.Helpers;
using APIFirstDemo.Models;
using System.Security.Principal;

namespace APIFirstDemo.Services
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private DemoDbContext _dbContext;
        private IBookRepository _book; 
        private ISortHelper<Book> _bookSortHelper; 
        private IDataShaper<Book> _bookDataShaper; 

        public IBookRepository Book
        {
            get
            {
                if (_book == null)
                {
                    _book = new BookRepository(_dbContext, _bookSortHelper, _bookDataShaper);
                }

                return _book;
            }
        }
         

        public RepositoryWrapper(DemoDbContext repositoryContext,
            ISortHelper<Book> bookSortHelper, 
            IDataShaper<Book> bookDataShaper)
        {
            _dbContext = repositoryContext;
            _bookSortHelper = bookSortHelper; 
            _bookDataShaper = bookDataShaper; 
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
