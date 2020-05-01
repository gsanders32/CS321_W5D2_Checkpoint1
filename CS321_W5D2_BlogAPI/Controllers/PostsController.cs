using System;
using CS321_W5D2_BlogAPI.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CS321_W5D2_BlogAPI.Core.Services;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CS321_W5D2_BlogAPI.Controllers
{
    [Authorize]
    // secure controller actions that change data
    [Route("api/[controller]")]
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        // inject PostService
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        //  get posts for blog
        //  allow anyone to get, even if not logged in
        // GET /api/blogs/{blogId}/posts
        [AllowAnonymous]
        [HttpGet("/api/blogs/{blogId}/posts")]
        public IActionResult Get(int blogId)
        {
            //  replace the code below with the correct implementation
            try
            {
                var blogs = _postService.GetBlogPosts(blogId);
                return Ok(blogs.ToApiModels());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ReturnBlogs", ex.Message);
                return BadRequest(ModelState);
            }
        }

        //  get post by id
        // allow anyone to get, even if not logged in
        // GET api/blogs/{blogId}/posts/{postId}
        [AllowAnonymous]
        [HttpGet("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Get(int blogId, int postId)
        {
            // replace the code below with the correct implementation
            try
            {
                var post = _postService
                    .GetBlogPosts(blogId)
                    .ToApiModels()
                    .FirstOrDefault(x => x.Id == postId);
                return Ok(post);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ReturnPosts", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // add a new post to blog
        // POST /api/blogs/{blogId}/post
        [HttpPost("/api/blogs/{blogId}/posts")]
        public IActionResult Post(int blogId, [FromBody]PostModel postModel)
        {
            //  replace the code below with the correct implementation

            try
            {               
                _postService.Add(postModel.ToDomainModel());
                return CreatedAtAction("Post", new { id = postModel.Id }, postModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AddPost", ex.Message);
                return BadRequest(ModelState);
            }
        }
            

        // PUT /api/blogs/{blogId}/posts/{postId}
        [HttpPut("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Put(int blogId, int postId, [FromBody]PostModel postModel)
        {
            try
            {
                var updatedPost = _postService.Update(postModel.ToDomainModel());
                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdatePost", ex.Message);
                return BadRequest(ModelState);
            }
        }

        //  delete post by id
        // DELETE /api/blogs/{blogId}/posts/{postId}
        [HttpDelete("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Delete(int blogId, int postId)
        {
            //  replace the code below with the correct implementation
            try
            {
                _postService.Remove(postId);
                return Ok(_postService.Get(blogId));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("DeletePost", "Fix Me! Implement DELETE /api/blogs{blogId}/posts/{postId}");
                return BadRequest(ModelState);
            }
            
        }
    }
}
