using System.ComponentModel.DataAnnotations;

namespace blog.Models.Domain
{
    /// <summary>
    /// Пост, Статья
    /// </summary>
    public class Post
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Загаловок
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } = null!;
        /// <summary>
        /// ID владелец поста
        /// </summary>
        public string UserGuid { get; set; } = null!;
    }
}
