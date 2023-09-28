namespace auth.Models
{
    /// <summary>
    /// Модел для входа
    /// </summary>
    public class LoginModel
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;

        public class Result
        {
            public string Login { get; set; }
            public string Token { get; set; }
        }
    }
}
