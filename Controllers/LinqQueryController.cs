using BlogApiLinq.Data;
using BlogApiLinq.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApiLinq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinqQueryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LinqQueryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            IEnumerable<Category> categories = from category in _context.Categories select category;
            return Ok(categories);
        }

        [HttpGet("GetPostWithTheirCategoryName")]
        public IActionResult GetPostWithTheirCategoryName()
        {
            var result = from post in _context.Posts 
                         select new
                         {
                             Title = post.Title,
                             Content = post.Content,
                             CategoryName = post.Category.Name
                         };
            return Ok(result);
        }

        [HttpGet("FindPostByTitle/{title}")]
        public IActionResult FindPostByTitle(string title)
        {
            var result = from post in _context.Posts
                         where post.Title == title
                         select new
                         {
                             Title = post.Title,
                             Content = post.Content,
                             CategoryName = post.Category.Name,
                             CreatedAT = post.CreatedAt
                         };

            return Ok(result);
        }

        [HttpGet("GetAllPostThatHaveSpecificTag/{tag}")]
        public IActionResult GetAllPostThatHaveSpecificTag(string tag) 
        {
            var result = from post in _context.Posts
                         where post.PostTags.Any(pt => pt.Tag.Name == tag)
                         select new
                         {
                             Title = post.Title,
                             Content = post.Content,
                             Tag = post.PostTags.Select(pt => pt.Tag.Name)
                         };
            return Ok(result);
        }

        [HttpGet("GetCountOfPostInEachCategory")]
        public IActionResult GetCountOfPostInEachCategory()
        {
            var result = from category in _context.Categories
                         select new
                         {
                             Name = category.Name,
                             PostCount = category.Posts.Count()
                         };

            return Ok(result);
        }

        [HttpGet("GetPostThatHaveNoComment")]
        public IActionResult GetPostThatHaveNoComment()
        {
            var result = from post in _context.Posts
                         where post.Comments.Any()
                         select new
                         {
                            Title = post.Title,
                            Content = post.Content,
                            CommentCount = post.Comments.Count()
                         };

            return Ok(result);
        }

        [HttpGet("GetLatestFivePost")]
        public IActionResult GetLatestFivePost()
        {
            var result = (from post in _context.Posts
                         orderby post.CreatedAt descending
                         select new
                         {
                             Title = post.Title,
                             CreatedAT = post.CreatedAt,
                         }).Take(5);

            return Ok(result);
        }
    }
}
