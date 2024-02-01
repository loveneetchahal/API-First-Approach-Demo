namespace APIFirstDemo.Services
{
    public interface IRepositoryWrapper
    {
        IBookRepository Book { get; }
        void Save();
    }
}
