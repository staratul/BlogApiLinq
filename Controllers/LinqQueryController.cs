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

        [HttpGet("GetAllCategoriesWithTheirPosts")]
        public IActionResult GetAllCategoriesWithTheirPosts()
        {
            var result = from category in _context.Categories
                         join post in _context.Posts
                         on category.Id equals post.CategoryId into postGroup
                         from post in postGroup.DefaultIfEmpty()
                         select new
                         {
                             CategroyName = category.Name,
                             PostTitle = post != null ? post.Title : string.Empty,
                         };

            return Ok(result);
        }

        [HttpGet("GetAllPostsWithTheirTags")]
        public IActionResult GetAllPostsWithTheirTags()
        {
            var result = from post in _context.Posts
                         join postTag in _context.PostTags
                         on post.Id equals postTag.PostId
                         join tag in _context.Tags
                         on postTag.TagId equals tag.Id
                         select new
                         {
                             post.Title,
                             TagName = tag.Name
                         };

            return Ok(result);
        }

        [HttpGet("GetTheMostCommentedPost")]
        public IActionResult GetTheMostCommentedPost()
        {
            var result = (from comment in _context.Comments
                          group comment by comment.PostId into grouped
                          orderby grouped.Count() descending
                          select new
                          {
                              PostId = grouped.Key,
                              CommentCount = grouped.Count(),
                          }).FirstOrDefault();

            return Ok(result);
        }

        [HttpGet("GetAllPostsThatHageNoTags")]
        public IActionResult GetAllPostsThatHageNoTags()
        {
            var result = from post in _context.Posts
                         join postTag in _context.PostTags
                         on post.Id equals postTag.PostId into postTags
                         from postTag in postTags.DefaultIfEmpty()
                         where postTag == null
                         select post;

            return Ok(result);
        }

        [HttpGet("GetAllPostsWithThierTags")]
        public IActionResult GetAllPostsWithThierTags()
        {
            var result = from post in _context.Posts
                         join postTag in _context.PostTags
                         on post.Id equals postTag.PostId into postTags
                         from postTag in postTags.DefaultIfEmpty()
                         join tag in _context.Tags
                         on postTag.TagId equals tag.Id into tags
                         from tag in tags.DefaultIfEmpty()
                         select new
                         {
                             post.Title,
                             TagName = tag != null ? tag.Name : string.Empty,
                         };

            return Ok(result);
        }

        [HttpGet("GetTheAverageNumberOfCommentsPerPosts")]
        public IActionResult GetTheAverageNumberOfCommentsPerPosts()
        {
            var result = (from comment in _context.Comments
                          group comment by comment.PostId into grouped
                          select grouped.Count()).Average();

            return Ok(result);
        }

        [HttpGet("GetAllCategoriesAndNumberOfPostsInEach")]
        public IActionResult GetAllCategoriesAndNumberOfPostsInEach()
        {
            var result = from category in _context.Categories
                         join post in _context.Posts
                         on category.Id equals post.CategoryId into posts
                         select new
                         {
                             CategoryName = category.Name,
                             PostCount = posts.Count(),
                         };

            return Ok(result);
        }


        [HttpGet("GetPostsWithTheirTotalCommentOrderByTheMostCommented")]
        public IActionResult GetPostsWithTheirTotalCommentOrderByTheMostCommented()
        {
            var postsByCommentCount = from post in _context.Posts
                         join comment in _context.Comments
                         on post.Id equals comment.PostId into comments
                         select new
                         {
                             post.Title,
                             CommentCount = comments.Count(),
                         } into result
                         orderby result.CommentCount descending
                         select result;

            return Ok(postsByCommentCount);
        }
    }
}
