using System;

namespace BlogApiLinq.Models;

public class Tag
{
    public int Id {get; set;}
    public string Name {get; set;}

    public List<PostTag> PostTags {get; set;} = new List<PostTag>();
}
