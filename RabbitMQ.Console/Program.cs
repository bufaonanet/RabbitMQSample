using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Console.Entities;

const string EXCHANGE = "curso-rabbitmq";

var connectionFactory = new ConnectionFactory { HostName = "localhost" };
var connection = connectionFactory.CreateConnection("RabbitMQ.Console");

#region Publicando mensagem

var channelPublish = connection.CreateModel();

var person = new Person("Douglas", "123.456.789", new DateTime(1986, 9, 21));
var json = JsonSerializer.Serialize(person);
var byteArray = Encoding.UTF8.GetBytes(json);

//channelPublish.BasicPublish(EXCHANGE, "hr.person-created", null, byteArray);
//Console.WriteLine($"Message published: {json}");

#endregion

#region Consumindo Mensagem

var channelConsumer = connection.CreateModel();
var consumer = new EventingBasicConsumer(channelConsumer);

consumer.Received += (sender, eventArgs) =>
{
    var contentArray = eventArgs.Body.ToArray();
    var contentString = Encoding.UTF8.GetString(contentArray);

    var message = JsonSerializer.Deserialize<Person>(contentString);

    Console.WriteLine($"Message received : {message}");

    channelPublish.BasicAck(eventArgs.DeliveryTag, multiple: false);
};

channelConsumer.BasicConsume("customer-created", autoAck: false, consumer);

#endregion


Console.ReadLine();