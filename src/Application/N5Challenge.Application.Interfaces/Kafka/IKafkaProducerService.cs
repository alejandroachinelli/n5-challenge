using N5Challenge.Application.DTO.DTOs.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.Interfaces.Kafka;

public interface IKafkaProducerService
{
    Task ProduceAsync(string topic, KafkaMessageDto message);
}