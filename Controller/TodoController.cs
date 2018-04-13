using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Persistence;
using TodoApi.Repo;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using TodoApi.Model;
using System.Threading.Tasks;
using System;

namespace TodoApi.Controllers
{
  [Route("api/todo")]
  public class TodoController : Controller
  {
    private readonly ITodoRepository _todoRepo;

    public TodoController(ITodoRepository todoRepo)
    {
      _todoRepo = todoRepo;
    }

    // [HttpHead]
    // public IActionResult GetHead()
    // {
    //   return Ok();
    // }

    [HttpGet]
    async public Task<IEnumerable<TodoItem>> GetAll()
    {
      return await _todoRepo.GetAllTodoItems();
    }

    [HttpGet("{id}", Name = "GetTodo")]
    async public Task<IActionResult> GetById(string id)
    {
      var item = await _todoRepo.GetTodoItem(id);
      if (item == null)
      {
        return NotFound();
      }
      return new ObjectResult(item);
    }

    [HttpPost]
    async public Task<IActionResult> Create([FromBody] TodoItem item)
    {
      if (item == null)
      {
        return BadRequest();
      }

      try
      {
        await _todoRepo.AddTodoItem(item);
        return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
      } catch (Exception)
      {
        return BadRequest();
      }
    }

    [HttpPut("{id}")]
    async public Task<IActionResult> Update(string id, [FromBody] TodoItem item)
    {
      if (item == null || item.Id != id)
      {
        return BadRequest();
      }

      var todo = await _todoRepo.GetTodoItem(id);
      if (todo == null)
      {
        return NotFound();
      }

      todo.IsComplete = item.IsComplete;
      todo.Name = item.Name;

      await _todoRepo.UpdateTodoItem(id, todo);
      return new NoContentResult();
    }

    [HttpPatch("{id}")]
    async public Task<IActionResult> Patch(string id, [FromBody] JsonPatchDocument<TodoItem> item)
    {
      var todo = await _todoRepo.GetTodoItem(id);
      if (todo == null)
      {
        return NotFound();
      }

      item.ApplyTo(todo);

      await _todoRepo.UpdateTodoItem(id, todo);
      return new NoContentResult();
    }

    [HttpDelete("{id}")]
    async public Task<IActionResult> Delete(string id)
    {
      var todo = await _todoRepo.GetTodoItem(id);
      if (todo == null)
      {
        return NotFound();
      }

      await _todoRepo.RemoveTodoItem(id);
      return new NoContentResult();
    }
  }
}
