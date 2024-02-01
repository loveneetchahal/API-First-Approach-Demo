using APIFirstDemo.Models;
using System.Dynamic;

namespace APIFirstDemo.Helpers
{
    public interface IDataShaper<T>
    {
        IEnumerable<Entity> ShapeData(IEnumerable<T> entities, string fieldsString);
        Entity ShapeData(T entity, string fieldsString);
    }
}
