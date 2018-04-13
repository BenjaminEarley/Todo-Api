using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApi.Model
{
  public class TodoItem
  {
    [BsonId]
    public ObjectId InternalId { get; set; }
    public String Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
  }
}