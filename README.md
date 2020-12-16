# Net.RabbitMQ
Package for publishing and consuming messages to RabbitMQ

## Instalation
Use the Nuget package manager [Nuget](https://www.nuget.org/packages/Net.RabbitMQ/) to install net.RabbitMQ

```bash
dotnet add package Net.RabbitMQ --version 1.0.1
```
## Usage

appsettings.json

```json

"RabbitMqConfiguraion": {
    "RabbitMqConnection": {
      "HostName": "localhost",
      "VirtualHost": "/",
      "Port": 5672,
      "UserName": "guest",
      "Password": "guest"
    },
    "Exchange": {
      "Name": "amq.direct",
      "Type": "direct"
    },
    "Queue": {
      "Name": "test",
      "Routing": "test",
      "Durable": "True",
      "AutoDelete": "false",
      "Exclusive": "false"
    },
    "PrefetchSize": 0,
    "PrefetchCount": 10
  }

```
DI Registration

```csharp
var config = _configuration.GetSection("RabbitMqConfiguraion").Get<RabbitMQConfiguration>();
services.AddRabbitMq(config);
```

Only Publisher 
```csharp
services.AddProducer(config);
```

Only Subscriber
```csharp
services.AddSubscriber(config);
```

Example Code
Publish
```csharp
_producer.Publish<T>(message);
```

Consume
```csharp
_consumer.Subscribe<T>(MessageProcessor);

private Task MessageProcessor(T arg)
{
    Console.WriteLine(arg);
    return Task.CompletedTask;
}
```
## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
