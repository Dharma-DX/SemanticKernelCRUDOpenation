using CrudWithNLP.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudWithNLP.Context
{
    public class NLPContext: DbContext
    {
        public DbSet<ToDo> ToDos => Set<ToDo>();

        public NLPContext(DbContextOptions<NLPContext> options) : base(options)
        {

        }
    }
}
