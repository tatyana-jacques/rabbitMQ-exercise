using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Send.Models;
using Receive;

var factory = new ConnectionFactory()
{
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};
using (var context = new TweetReceiveContext())
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())

{

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Tweet tweet = JsonConvert.DeserializeObject<Tweet>(message);
        context.Tweets.Add(tweet);
        context.SaveChangesAsync();

        Console.WriteLine($"{tweet.Name}; {tweet.Text}");
    };



    channel.BasicConsume(queue: "tatyana",
                            autoAck: true,
                            consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}