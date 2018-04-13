using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using TodoApi.Model;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using TodoApi.Persistence;

namespace TodoApi.Repo
{
  public class TodoRepository : ITodoRepository
  {
    private readonly TodoContext _context = null;

    public TodoRepository(IOptions<Settings> settings)
    {
      _context = new TodoContext(settings);
    }

    public async Task<IEnumerable<TodoItem>> GetAllTodoItems()
    {
      try
      {
        return await _context.TodoItems
                .Find(_ => true).ToListAsync();
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    // query after Id or InternalId (BSonId value)
    //
    public async Task<TodoItem> GetTodoItem(string id)
    {
      try
      {
        ObjectId internalId = GetInternalId(id);
        return await _context.TodoItems
                        .Find(todoItem => todoItem.Id == id
                                || todoItem.InternalId == internalId)
                        .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    private ObjectId GetInternalId(string id)
    {
      ObjectId internalId;
      if (!ObjectId.TryParse(id, out internalId))
        internalId = ObjectId.Empty;

      return internalId;
    }

    public async Task AddTodoItem(TodoItem item)
    {
      try
      {
        await _context.TodoItems.InsertOneAsync(item);
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<bool> RemoveTodoItem(string id)
    {
      try
      {
        DeleteResult actionResult
            = await _context.TodoItems.DeleteOneAsync(
                Builders<TodoItem>.Filter.Eq("Id", id));

        return actionResult.IsAcknowledged
            && actionResult.DeletedCount > 0;
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<bool> UpdateTodoItem(string id, string name)
    {
      var filter = Builders<TodoItem>.Filter.Eq(s => s.Id, id);
      var update = Builders<TodoItem>.Update
                      .Set(s => s.Name, name)
                      .CurrentDate(s => s.UpdatedOn);

      try
      {
        UpdateResult actionResult
            = await _context.TodoItems.UpdateOneAsync(filter, update);

        return actionResult.IsAcknowledged
            && actionResult.ModifiedCount > 0;
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<bool> UpdateTodoItem(string id, bool isComplete)
    {
      var filter = Builders<TodoItem>.Filter.Eq(s => s.Id, id);
      var update = Builders<TodoItem>.Update
                      .Set(s => s.IsComplete, isComplete)
                      .CurrentDate(s => s.UpdatedOn);

      try
      {
        UpdateResult actionResult
            = await _context.TodoItems.UpdateOneAsync(filter, update);

        return actionResult.IsAcknowledged
            && actionResult.ModifiedCount > 0;
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<bool> UpdateTodoItem(string id, TodoItem todoItem)
    {
      try
      {
        ReplaceOneResult actionResult
            = await _context.TodoItems
                            .ReplaceOneAsync(n => n.Id.Equals(id)
                                    , todoItem
                                    , new UpdateOptions { IsUpsert = true });
        return actionResult.IsAcknowledged
            && actionResult.ModifiedCount > 0;
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<bool> UpdateTodoDocument(string id, string name, bool isComplete)
    {
      var todoItem = await GetTodoItem(id) ?? new TodoItem();
      todoItem.Name = name;
      todoItem.IsComplete = isComplete;
      todoItem.UpdatedOn = DateTime.Now;

      return await UpdateTodoItem(id, todoItem);
    }

    public async Task<bool> RemoveAllTodoItems()
    {
      try
      {
        DeleteResult actionResult
            = await _context.TodoItems.DeleteManyAsync(new BsonDocument());

        return actionResult.IsAcknowledged
            && actionResult.DeletedCount > 0;
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }
  }
}