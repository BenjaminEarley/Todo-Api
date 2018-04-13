using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Model;

namespace TodoApi.Repo
{
  public interface ITodoRepository
  {
    Task<IEnumerable<TodoItem>> GetAllTodoItems();
    Task<TodoItem> GetTodoItem(string id);

    // add new note document
    Task AddTodoItem(TodoItem item);

    // remove a single document 
    Task<bool> RemoveTodoItem(string id);

    // update just a single document 
    Task<bool> UpdateTodoItem(string id, string name);

    // update just a single document 
    Task<bool> UpdateTodoItem(string id, bool isComplete);

    // full document update
    Task<bool> UpdateTodoDocument(string id, string name, bool isComplete);

    Task<bool> UpdateTodoItem(string id, TodoItem todoItem);

    // should be used with high caution
    Task<bool> RemoveAllTodoItems();
  }
}