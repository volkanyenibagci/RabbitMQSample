using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQSample.Models;

namespace RabbitMQSample.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var factory =new ConnectionFactory(){HostName="localhost"};
        using(var connection=factory.CreateConnection())
        using(var channel=connection.CreateModel())
        {
            channel.QueueDeclare(
            queue:"Hello",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
            string message="Hello World!";
            var body=Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: "hello",
                basicProperties: null,
                body: body);
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
