using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _dbContext;
        public PostRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public Post Get(int id)
        {
            //  Implement Get(id). Include related Blog and Blog.User
            Post post = _dbContext.Posts.Include(x => x.Blog).Include(x => x.Blog.User).FirstOrDefault(x => x.Id == id);
            if (post != null)
            {
                return post;
            }
            return null;
        }

        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            //  Implement GetBlogPosts, return all posts for given blog id
            //  Include related Blog and AppUser
            var List = _dbContext.Posts.Include(x => x.Blog).Include(x => x.Blog.User).Where(x => x.BlogId == blogId).ToList();
            if (List != null)
            {
                return List;
            }
            return null;
        }

        public Post Add(Post Post)
        {
            //  add Post
            _dbContext.Add(Post);
            _dbContext.SaveChanges();
            return Post;
        }

        public Post Update(Post updatedPost)
        {
            var post = _dbContext.Posts.Find(updatedPost.Id);
            if (post == null) return null;
            _dbContext.Entry(post)
               .CurrentValues
               .SetValues(updatedPost);
            _dbContext.Posts.Update(post);
            _dbContext.SaveChanges();
            return post;

        }

        public IEnumerable<Post> GetAll()
        {
            //  get all posts
            return _dbContext.Posts.ToList();
        }

        public void Remove(int id)
        {
            //  remove Post
            Post findPost = _dbContext.Posts.Find(id);

            if (findPost != null)
            {
                _dbContext.Posts.Remove(findPost);
                _dbContext.SaveChanges();
            }
        }

    }
}
