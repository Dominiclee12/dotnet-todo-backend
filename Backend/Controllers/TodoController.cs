using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext context;

        public TodoController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            // var username = User.Identity?.Name;
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var todos = await context.Todos.Where(x => x.UserId == userId).ToListAsync();

            return Ok(todos);
        }

        [HttpPost]
        public async Task<IActionResult> AddTodo(TodoDto todoDto)
        {
            if (!ModelState.IsValid || todoDto.Title.IsNullOrEmpty())
                return BadRequest();

            var newTodo = new Todo
            {
                Title = todoDto.Title,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            };
            context.Todos.Add(newTodo);
            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "Successfully created",
                todo = newTodo
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoCompleted(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var todoUpdate = await context.Todos.FindAsync(id);

            if (todoUpdate == null)
                return NotFound();

            if (todoUpdate.UserId != userId)
                return Unauthorized();

            todoUpdate.Completed = !todoUpdate.Completed;

            context.Update(todoUpdate);
            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "Successfully updated",
                todo = todoUpdate
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var deleteTodo = await context.Todos.FindAsync(id);

            if (deleteTodo == null)
                return NotFound();

            if (deleteTodo.UserId != userId)
                return Unauthorized();

            context.Remove(deleteTodo);
            await context.SaveChangesAsync();

            return Ok("Successfully deleted");
        }
    }
}
