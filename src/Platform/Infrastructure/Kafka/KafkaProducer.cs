using System;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Kafka;

public interface IKafkaProducer
{
    Task ProduceAsync(string topic, string payload);
}

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(IConfiguration config)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public Task ProduceAsync(string topic, string payload)
    {
        return _producer.ProduceAsync(topic, new Message<Null, string> { Value = payload });
    }
}
