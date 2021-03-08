# Net.RabbitMQ
Package for publishing and consuming messages to RabbitMQ

## Instalation
Use the Nuget package manager [Nuget](https://www.nuget.org/packages/Net.RabbitMQ/) to install net.RabbitMQ

```bash
dotnet add package Net.RabbitMQ --version 1.0.8
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
      "Name": "testexchange",
      "Type": "direct"
    },
    "DLExchange": {
      "Name": "dltestexchange",
      "Type": "direct"
    },
    "Queue": {
      "Name": "queue",
      "Durable": "True",
      "AutoDelete": "false",
      "Exclusive": "false"
    },
    "DLQueue": {
      "Name": "dlqueue",
      "Durable": "True",
      "AutoDelete": "false",
      "Exclusive": "false"
    },
    "Routing": "test",
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
_producer.PublishAsync<T>(message); // IProducer interface
```

Consume
```csharp
_consumer.SubscribeAsync<T>(MessageProcessor); // IConsumer interface

private Task MessageProcessor(T arg)
{
    Console.WriteLine(arg);
    return Task.CompletedTask;
}
```