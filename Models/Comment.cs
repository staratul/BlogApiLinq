using System;

namespace BlogApiLinq.Models;

public class Comment
{
    public int Id {get; set;}
    public string Content {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public int PostId {get; set;}
    public Post Post {get; set;}
}
