using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using N5Challenge.Application.DTO.DTOs.Kafka;
using N5Challenge.Application.Interfaces.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace N5Challenge.Infrastructure.Services.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerService(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092"
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, KafkaMessageDto message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
    }
}