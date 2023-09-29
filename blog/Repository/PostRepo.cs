using blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace blog.Repository;

/// <summary>
/// Post Interface
/// </summary>
public interface IPostRepo
{
    Task<Post> GetPostById(int Id);
    Task<List<Post>> GetAllPosts(string userId);
    Task AddPostAsync(string title, string description, string userguid);
    void DeletePost(int Id);
    void UpdatePost(Post post);
}

/// <summary>
/// Post Repo
/// </summary>
public class PostRepo : IPostRepo
{
    private readonly BlogDbContext _dbContext;

    public PostRepo(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public async Task AddPostAsync(string title, string description, string usergui)
    {
        /// Logics
        await _dbContext.Posts.AddAsync(new Post { Title = title, Description = description, UserGuid = usergui });
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    public void DeletePost(int Id)
    {
        Post post = _dbContext.Posts.Single(p => p.Id == Id);
        _dbContext.Remove(post);
        _dbContext.SaveChanges();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<Post>> GetAllPosts(string userId)
    {
        return await _dbContext.Posts
            .Where(p => p.UserGuid == userId)
            .ToListAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public async Task<Post> GetPostById(int Id)
    {
        return await _dbContext.Posts.SingleAsync(p => p.Id == Id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="post"></param>
    public void UpdatePost(Post post)
    {
        _dbContext.Posts.Update(post);
        _dbContext.SaveChanges();
    }
}
