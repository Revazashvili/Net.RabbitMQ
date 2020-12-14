namespace Net.RabbitMQ.Models.Primitives
{
    public interface IUserInfo
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}