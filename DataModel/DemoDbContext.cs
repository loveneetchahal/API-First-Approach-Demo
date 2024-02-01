using APIFirstDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Principal;

namespace APIFirstDemo.DataModel
{
    public class DemoDbContext: DbContext
    {
        public DemoDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } 
    }
}
