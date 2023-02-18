using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};


using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "ex1",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

    string message = "Tatyana";

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "",
                        routingKey: "ex1",
                        basicProperties: null,
                        body: body);
    Console.WriteLine(message);
}

Console.WriteLine(" Pressione [enter] para sair.");
Console.ReadLine();
