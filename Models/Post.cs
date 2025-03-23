using System;

namespace BlogApiLinq.Models;

public class Post
{
    public int Id { get; set;}
    public string Title { get; set;}

    public string Content {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }

    public Category Category {get; set;}

    public List<PostTag> PostTags {get; set;} = new List<PostTag>();
    public List<Comment> Comments {get; set;} = new List<Comment>();
}
