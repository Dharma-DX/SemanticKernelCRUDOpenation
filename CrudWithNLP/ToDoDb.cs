using Microsoft.EntityFrameworkCore;

namespace CrudWithNLP
{
    public class ToDoDb :DbContext
    {
        public DbSet<ToDo> Todos => Set<ToDo>(); 
        public ToDoDb(DbContextOptions<ToDoDb> options): base(options)
        {
                
        }
    }
}
