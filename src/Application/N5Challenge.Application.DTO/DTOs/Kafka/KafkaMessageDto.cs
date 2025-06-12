using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.DTO.DTOs.Kafka;

public class KafkaMessageDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Operation { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}