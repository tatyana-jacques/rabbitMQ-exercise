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
    channel.QueueDeclare(queue: "tatyana",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

    string message = "aí sim";

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "",
                        routingKey: "tatyana",
                        basicProperties: null,
                        body: body);
    Console.WriteLine(message);

}

Console.WriteLine(" Pressione [enter] para sair.");
Console.ReadLine();