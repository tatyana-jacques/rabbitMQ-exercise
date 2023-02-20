using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

try
{

    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();

    channel.ExchangeDeclare("chat", ExchangeType.Fanout);

    channel.QueueDeclare(queue: "chat-tatyjacques",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

    channel.QueueBind(queue: "chat-tatyjacques",
                            exchange: "chat",
                            routingKey: "");



    RecebeMensagensChat(channel);



    Console.WriteLine("Escreva seu nome: ");

    var nome = Console.ReadLine();
    var message = "";
    do
    {
        Console.WriteLine("Para sair, escreva: /sair");
        message = Console.ReadLine();

        if (message != "/sair")
        {

            var body = Encoding.UTF8.GetBytes($"[{nome}]: {message}");

            channel.BasicPublish(exchange: "chat",
                            routingKey: "",
                            basicProperties: null,
                            body: body);
        }

    } while (message != "/sair");

}
catch (Exception e)
{

}
Console.WriteLine(" Digite [enter] para sair.");
Console.ReadLine();


static void RecebeMensagensChat(IModel channel)
{
    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(message);
    };
    channel.BasicConsume(queue: "chat-tatyjacques",
                            autoAck: false,
                            consumer: consumer);
}