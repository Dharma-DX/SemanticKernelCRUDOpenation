using CrudWithNLP.Context;
using CrudWithNLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CrudWithNLP.Controllers
{
    [Route("[controller]/[action]")]
    public class ToDoController : ControllerBase
    {
        private NLPContext _nlpContext; 

        public ToDoController(NLPContext context)
        {
            _nlpContext = context;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDo input)
        {
            try
            {
                //ToDo todo = JsonConvert.DeserializeObject<ToDo>(input);
                var result = _nlpContext.ToDos.Add(input);
                await _nlpContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok("ToDo added successfully");
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _nlpContext.ToDos.ToListAsync();
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCompleteStatus()
        {
            var result = await _nlpContext.ToDos.Where(x => x.IsComplete).ToListAsync();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var result = await _nlpContext.ToDos.FindAsync(id);
            return Ok(result);
        }


        [HttpPut]
        public async Task<IActionResult> Update(ToDo input)
        {
            var result = await _nlpContext.ToDos.FindAsync(input.Id);
            if (result == null)
            {
                return NotFound();
            }
            result.Name = input.Name;
            result.IsComplete = input.IsComplete;

            await _nlpContext.SaveChangesAsync();
            return Ok("Updated successfully");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _nlpContext.ToDos.FindAsync(id);
            _nlpContext.ToDos.Remove(result);
            await _nlpContext.SaveChangesAsync();
            return Ok("Removed successfully");
        }
    }
}
