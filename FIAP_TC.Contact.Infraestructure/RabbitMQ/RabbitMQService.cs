using FIAP_TC.Contact.Core.Entities;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FIAP_TC.Contact.Infraestructure.RabbitMQ
{
    public interface IRabbitMQService
    {
        public bool CreateContact(Contato contato);
        bool CheckConnection();
    }

    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;

        public RabbitMQService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true
            };

            _connection = factory.CreateConnection();
        }

        public bool CreateContact(Contato contato)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(
                queue: "contatos-criados",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = JsonSerializer.Serialize(contato);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: "contatos-criados",
                basicProperties: null,
                body: body
            );

            return true;
        }

        public bool CheckConnection()
        {
            return _connection != null && _connection.IsOpen;
        }
    }

}
