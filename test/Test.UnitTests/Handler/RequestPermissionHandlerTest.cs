using AutoMapper;
using Moq;
using N5Challenge.Application.DTO.DTOs.Kafka;
using N5Challenge.Application.Interfaces.ElasticSearch;
using N5Challenge.Application.Interfaces.Kafka;
using N5Challenge.Application.Interfaces.Persistence;
using N5Challenge.Application.UseCases.Permission.Commands.RequestPermission;
using N5Challenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.UnitTests.Handler;

public class RequestPermissionHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IKafkaProducerService> _kafkaMock = new();
    private readonly Mock<IElasticsearchService> _elasticsearchMock = new();

    [Fact]
    public async Task Handle_ShouldCreatePermission_WhenValidRequest()
    {
        // Arrange
        var command = new RequestPermissionCommand("Juan", "Pérez", 1, DateTime.Now);

        var handler = new RequestPermissionHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _kafkaMock.Object,
            _elasticsearchMock.Object
        );

        _unitOfWorkMock
            .Setup(u => u.PermissionRepository.AddAsync(It.IsAny<PermissionEntity>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Permiso creado exitosamente", result.Message);
        _unitOfWorkMock.Verify(u => u.PermissionRepository.AddAsync(It.IsAny<PermissionEntity>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _kafkaMock.Verify(k => k.ProduceAsync("permissions-topic", It.IsAny<KafkaMessageDto>()), Times.Once);
        _elasticsearchMock.Verify(e => e.IndexPermissionAsync(It.IsAny<PermissionEntity>()), Times.Once);
    }
}