using APIFirstDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIFirstDemo.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        //https://github.com/Microsoft/api-guidelines/blob/vNext/Guidelines.md

        private readonly List<Product> _products;
        public ProductsController()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.99m, Description = "Description 1", Category = "Category 1" },
                new Product { Id = 2, Name = "Product 2", Price = 20.99m, Description = "Description 2", Category = "Category 2" },
                new Product { Id = 3, Name = "Product 3", Price = 30.99m, Description = "Description 3", Category = "Category 3" },
                new Product { Id = 4, Name = "Product 4", Price = 40.99m, Description = "Description 4", Category = "Category 4" },
                new Product { Id = 5, Name = "Product 5", Price = 10.99m, Description = "Description 5", Category = "Category 5" },
                new Product { Id = 6, Name = "Product 6", Price = 20.99m, Description = "Description 6", Category = "Category 6" },
                new Product { Id = 7, Name = "Product 7", Price = 30.99m, Description = "Description 7", Category = "Category 7" },
                new Product { Id = 8, Name = "Product 8", Price = 40.99m, Description = "Description 8", Category = "Category 8" },
            };
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _products;
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public Product Get(int id)
        {
            return _products.Find(e => e.Id == id);
        }
        // POST: api/Products
        [HttpPost]
        [Produces("application/json")]
        public Product Post([FromBody] Product Products)
        {
            // Logic to create new Products
            return new Product();
        }
        // PUT: api/Products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product Products)
        {
            // Logic to update an Products
        }
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

        /*
         * 
         * In the GetProducts action method, we receive the following parameters from the query string: 
            fields: To select specific fields in the response.
            searchTerm: To filter products based on a search term.
            sortBy: To specify the sorting order.
            page and pageSize: To implement pagination.
            We apply the respective filters, sorting, and pagination to the queryable collection of products. Finally, 
                    we perform data shaping by selecting the desired fields based on the fields parameter.

            Step 3: Test the API
            You can now run the application and test the API using tools like Postman or curl. Here are some example requests:

            GET /api/products: Retrieve all products with default sorting and pagination.
            GET /api/products?fields=Id,Name,Price: Retrieve products with only the specified fields (e.g., Id, Name, and Price).
            GET /api/products?searchTerm=Product&sortBy=Name&page=2&pageSize=5: Retrieve products containing 
            the search term "Product," sorted by name, and retrieve the second page with 5 products per page.
         * */

        [HttpGet]
        [Route("GetProducts")]
        public ActionResult<IEnumerable<Product>> GetProducts(
            [FromQuery] string fields,
            [FromQuery] string searchTerm,
            [FromQuery] string sortBy,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            IQueryable<Product> query = _products.AsQueryable();

            // Filter by search term
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Sorting logic here based on the sortBy parameter
                // For simplicity, let's assume sorting by name in ascending order
                query = query.OrderBy(p => p.Name);
            }

            // Pagination
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            // Data shaping
            if (!string.IsNullOrWhiteSpace(fields))
            {
                var propertyNames = fields.Split(',');

                query = query.Select(p => new Product
                {
                    Id = p.Id,
                    Name = propertyNames.Contains("Name") ? p.Name : null,
                    Price = propertyNames.Contains("Price") ? p.Price : 0,
                    Description = propertyNames.Contains("Description") ? p.Description : null,
                    Category = propertyNames.Contains("Category") ? p.Category : null
                });
            }

            var products = query.ToList();

            return Ok(products);
        }


        /*
         * 
         * GET /api/products?filter=Name:eq:Product 1: Filter products where the Name field is equal to "Product 1".
            GET /api/products?filter=Price:gt:20: Filter products where the Price field is greater than 20.
            GET /api/products?filter=Price:lt:30,Category:eq:Category 1: Filter products where the Price field is less than 30 and the Category field is equal to "Category 1".
         */
        [HttpGet]
        [Route("GetProductsWithOperator")]
        public ActionResult<IEnumerable<Product>> GetProducts(
            [FromQuery] string filter)
        {
            IQueryable<Product> query = _products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = ApplyFilters(query, filter);
            }

            var products = query.ToList();

            return Ok(products);
        }

        private IQueryable<Product> ApplyFilters(IQueryable<Product> query, string filter)
        {
            var filters = filter.Split(',');

            foreach (var filterExpression in filters)
            {
                var tokens = filterExpression.Split(':');

                if (tokens.Length != 3)
                {
                    // Invalid filter expression
                    continue;
                }

                var fieldName = tokens[0];
                var comparisonOperator = tokens[1];
                var value = tokens[2];

                switch (comparisonOperator)
                {
                    case "eq": // Equals
                        query = query.Where(p => GetPropertyValue(p, fieldName).ToString() == value);
                        break;
                    case "gt": // Greater than
                        query = query.Where(p => (decimal)GetPropertyValue(p, fieldName) > decimal.Parse(value));
                        break;
                    case "lt": // Less than
                        query = query.Where(p => (decimal)GetPropertyValue(p, fieldName) < decimal.Parse(value));
                        break;
                    // Add more comparison operators as needed

                    default:
                        // Invalid comparison operator
                        continue;
                }
            }

            return query;
        }

        private object GetPropertyValue(Product product, string fieldName)
        {
            return product.GetType().GetProperty(fieldName)?.GetValue(product);
        }
    }
}