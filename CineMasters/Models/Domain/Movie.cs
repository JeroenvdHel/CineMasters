using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineMasters.Models.Domain
{
    public class Movie
    {
    [BsonId]
    public ObjectId InternalId { get; set; }
    
    public long Id { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    }
}