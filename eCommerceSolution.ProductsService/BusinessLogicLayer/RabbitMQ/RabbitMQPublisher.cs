
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eCommerce.ProductsService.BusinessLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
        Console.WriteLine($"Connecting to(product microservice) RabbitMQ at {_configuration["RabbitMQ_HostName"]}:{_configuration["RabbitMQ_Port"]} with user {_configuration["RabbitMQ_UserName"]!}");

        string hostName = _configuration["RabbitMQ_HostName"]!;
        string userName = _configuration["RabbitMQ_UserName"]!;
        string password = _configuration["RabbitMQ_Password"]!;
        string port = _configuration["RabbitMQ_Port"]!;


        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = Convert.ToInt32(port)
        };

        int retryCount = 5;
        while (retryCount > 0)
        {
            try
            {
                _connection = connectionFactory.CreateConnection();
                break;
            }
            catch (global::RabbitMQ.Client.Exceptions.BrokerUnreachableException)
            {
                retryCount--;
                if (retryCount == 0) throw;
                System.Threading.Thread.Sleep(5000); // Wait 5 seconds before retrying
            }
        }

        _channel = _connection.CreateModel();
    }


    public void Publish<T>(string routingKey, T message)
    {
        string messageJson = JsonSerializer.Serialize(message);
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

        //Create exchange
        string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

        //Publish message
        _channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: messageBodyInBytes);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}