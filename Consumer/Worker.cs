using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using FIAP_TC.Case.Consumer.Entities;
using FIAP_TC.Case.Consumer.Data;
using Microsoft.IdentityModel.Tokens;

namespace Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            // Estabelece a conexão e o canal para o RabbitMQ
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara a fila
            _channel.QueueDeclare(
                queue: "contatos-criados",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _logger.LogInformation("Conexão com RabbitMQ estabelecida e fila declarada.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    // Deserializa a mensagem
                    var contato = JsonSerializer.Deserialize<Contato>(message);

                    if (contato == null || contato.Id <= 0)
                    {
                        _logger.LogWarning("Mensagem deserializada inválida ou contato sem ID.");
                        _channel.BasicReject(eventArgs.DeliveryTag, false); // Rejeita mensagem inválida
                        return;
                    }

                    _logger.LogInformation($"Mensagem recebida: {message}");

                    // Processa o contato
                    await ProcessarContato(contato, stoppingToken);

                    // Confirma a mensagem no RabbitMQ
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao processar mensagem: {ex.Message}");
                    _channel.BasicNack(eventArgs.DeliveryTag, false, true); // Reencaminha a mensagem para a fila
                }
            };

            _channel.BasicConsume(
                queue: "contatos-criados",
                autoAck: false,
                consumer: consumer);

            // Mantém o serviço em execução
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ProcessarContato(Contato contato, CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DbTcContext>();

            var atendimento = new Atendimento
            {
                Assunto = "Primeiro Atendimento",
                DataModificacao = DateTime.Now,
                DataSolicitacao = DateTime.Now,
                Descricao = "Contato recem criado",
                IdContato = contato.Id
            };

            // Insere o atendimento no banco de dados
            await dbContext.AddAsync(atendimento, stoppingToken);
            await dbContext.SaveChangesAsync(stoppingToken);

            _logger.LogInformation($"Atendimento para o contato {contato.Id} criado com sucesso.");
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
