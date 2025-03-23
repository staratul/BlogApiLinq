using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlogApiLinq.Models; // Ensure correct namespace
using System.Collections.Generic;

namespace BlogApiLinq.Data
{
    public static class DataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Apply migrations and ensure database is created
                context.Database.Migrate();

                // Seed Categories
                if (!context.Categories.Any())
                {
                    var categories = new List<Category>
                    {
                        new Category { Name = "Technology" },
                        new Category { Name = "Lifestyle" },
                        new Category { Name = "Education" }
                    };
                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                }

                // Retrieve Categories (Ensuring IDs are assigned correctly)
                var categoriesList = context.Categories.ToList();
                var techCategory = categoriesList.FirstOrDefault(c => c.Name == "Technology")?.Id ?? 1;
                var lifestyleCategory = categoriesList.FirstOrDefault(c => c.Name == "Lifestyle")?.Id ?? 2;
                var educationCategory = categoriesList.FirstOrDefault(c => c.Name == "Education")?.Id ?? 3;

                // Seed Posts
                if (!context.Posts.Any())
                {
                    var posts = new List<Post>
                    {
                        new Post { Title = "Latest Tech Trends", Content = "AI, Blockchain, and more", CategoryId = techCategory },
                        new Post { Title = "Healthy Living Tips", Content = "Eat well, sleep well", CategoryId = lifestyleCategory },
                        new Post { Title = "Learning C# Basics", Content = "Start your journey in C#", CategoryId = educationCategory }
                    };
                    context.Posts.AddRange(posts);
                    context.SaveChanges();
                }

                // Retrieve Posts
                var postsList = context.Posts.ToList();
                var techPost = postsList.FirstOrDefault(p => p.Title == "Latest Tech Trends")?.Id ?? 1;
                var healthPost = postsList.FirstOrDefault(p => p.Title == "Healthy Living Tips")?.Id ?? 2;
                var csharpPost = postsList.FirstOrDefault(p => p.Title == "Learning C# Basics")?.Id ?? 3;

                // Seed Tags
                if (!context.Tags.Any())
                {
                    var tags = new List<Tag>
                    {
                        new Tag { Name = "AI" },
                        new Tag { Name = "Blockchain" },
                        new Tag { Name = "Health" },
                        new Tag { Name = "Programming" }
                    };
                    context.Tags.AddRange(tags);
                    context.SaveChanges();
                }

                // Retrieve Tags
                var tagsList = context.Tags.ToList();
                var aiTag = tagsList.FirstOrDefault(t => t.Name == "AI")?.Id ?? 1;
                var blockchainTag = tagsList.FirstOrDefault(t => t.Name == "Blockchain")?.Id ?? 2;
                var healthTag = tagsList.FirstOrDefault(t => t.Name == "Health")?.Id ?? 3;
                var programmingTag = tagsList.FirstOrDefault(t => t.Name == "Programming")?.Id ?? 4;

                // Seed PostTags (Many-to-Many relationship)
                if (!context.PostTags.Any())
                {
                    var postTags = new List<PostTag>
                    {
                        new PostTag { PostId = techPost, TagId = aiTag },
                        new PostTag { PostId = techPost, TagId = blockchainTag },
                        new PostTag { PostId = healthPost, TagId = healthTag },
                        new PostTag { PostId = csharpPost, TagId = programmingTag }
                    };
                    context.PostTags.AddRange(postTags);
                    context.SaveChanges();
                }

                // Seed Comments
                if (!context.Comments.Any())
                {
                    var comments = new List<Comment>
                    {
                        new Comment { Content = "Great post!", PostId = techPost },
                        new Comment { Content = "Very informative.", PostId = healthPost },
                        new Comment { Content = "Thanks for sharing!", PostId = csharpPost }
                    };
                    context.Comments.AddRange(comments);
                    context.SaveChanges();
                }
            }
        }
    }
}
