using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApi.Persistence;
using TodoApi.Model;

namespace TodoApi.Persistence
{
  public class TodoContext
  {
    private readonly IMongoDatabase _database = null;

    public TodoContext(IOptions<Settings> settings)
    {
      var client = new MongoClient(settings.Value.ConnectionString);
      if (client != null)
        _database = client.GetDatabase(settings.Value.Database);
    }

    public IMongoCollection<TodoItem> TodoItems
    {
      get
      {
        return _database.GetCollection<TodoItem>("TodoItems");
      }
    }
  }
}
