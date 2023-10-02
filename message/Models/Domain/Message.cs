namespace message.Models.Domain;



public class Message
{
    public string Id { get; set; }
    /// <summary>
    /// Текст сообщение
    /// </summary>
    public string MessageText { get; set; }
    /// <summary>
    /// Отправитель сообщение
    /// </summary>
    public string UserId { get; set; } = null!;

}
