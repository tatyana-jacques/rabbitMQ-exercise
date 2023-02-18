﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System;

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

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(message.ToUpper());
    };

    channel.BasicConsume(queue: "tatyana",
                            autoAck: true,
                            consumer: consumer);

    Console.WriteLine(" Pressione [enter] para sair.");
    Console.ReadLine();
}